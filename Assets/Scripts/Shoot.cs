using UnityEngine;
using Vuforia;


public class Shoot : MonoBehaviour
{

    public class VuforiaGameController : MonoBehaviour
    {
        public GameObject sunPrefab;          // The Sun object (the gun)
        public GameObject earthPrefab;        // Earth object (the target)
        public GameObject solarFlarePrefab;   // The solar flare projectile
        public int numberOfEarths = 5;        // Number of Earths to spawn
        public float spawnRadius = 5f;        // Radius around the player to spawn Earths
        public float solarFlareSpeed = 10f;   // Speed of the solar flare
        public GameObject auroraPrefab;       // The aurora effect that plays on hit
        public int score = 0;                 // Player score
        public TMPro.TextMeshProUGUI scoreText;  // UI element to display score

        private Camera arCamera;
        private bool isTracking = false;      // To track if the image target is found (optional for image target based)

        void Start()
        {
            // Get AR Camera
            arCamera = Camera.main;

            // If using Ground Plane, spawn Earths immediately
            if (numberOfEarths > 0)
            {
                SpawnEarthsAroundPlayer();
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootSolarFlare();
            }
        }

        // Call this when image tracking is detected
        public void OnTrackingFound()
        {
            isTracking = true;
            SpawnEarthsAroundPlayer();
        }

        void SpawnEarthsAroundPlayer()
        {
            if (!isTracking) return;

            Vector3 playerPosition = arCamera.transform.position;

            for (int i = 0; i < numberOfEarths; i++)
            {
                Vector3 randomPosition = playerPosition + Random.insideUnitSphere * spawnRadius;
                randomPosition.y = Mathf.Max(randomPosition.y, 0.1f); // Ensure Earth spawns above the ground
                Instantiate(earthPrefab, randomPosition, Quaternion.identity);
            }
        }

        void ShootSolarFlare()
        {
            Ray ray = new Ray(arCamera.transform.position, arCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Earth"))
                {
                    // Instantiate solar flare and move towards the Earth
                    GameObject flare = Instantiate(solarFlarePrefab, arCamera.transform.position, Quaternion.identity);
                    StartCoroutine(MoveSolarFlare(flare, hit.point, hit.collider.gameObject));
                }
            }
        }

        System.Collections.IEnumerator MoveSolarFlare(GameObject flare, Vector3 targetPosition, GameObject hitEarth)
        {
            while (Vector3.Distance(flare.transform.position, targetPosition) > 0.1f)
            {
                flare.transform.position = Vector3.MoveTowards(flare.transform.position, targetPosition, Time.deltaTime * solarFlareSpeed);
                yield return null;
            }

            Destroy(flare); // Destroy the solar flare when it reaches the Earth

            // Trigger the aurora effect and destroy the Earth
            TriggerAuroraAndDestroyEarth(hitEarth, targetPosition);
        }

        void TriggerAuroraAndDestroyEarth(GameObject earth, Vector3 hitPosition)
        {
            // Play aurora effect at the hit position
            GameObject aurora = Instantiate(auroraPrefab, hitPosition, Quaternion.identity);
            Destroy(aurora, 3f);  // Destroy the aurora effect after 3 seconds

            // Destroy the Earth
            Destroy(earth);

            // Increment score and update the UI
            score += 100;
            UpdateScoreUI();
        }

        void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }

}
