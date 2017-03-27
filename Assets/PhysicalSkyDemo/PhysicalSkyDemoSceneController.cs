using System;
using UnityEngine;
using PhysicalSky.Utilities;

namespace PhysicalSky
{
    public class PhysicalSkyDemoSceneController : SunController
    {
        [SerializeField]
        protected float latitude = 45.5017f;
        [SerializeField]
        protected float longitude = -73.5673f;
        [SerializeField]
        protected DateTime time = DateTime.Now;
        [SerializeField]
        protected TimeSpan timezone = TimeSpan.FromHours(5); // EST
        [SerializeField]
        protected float minutesPerHour = 60.0f;
        [SerializeField]
        protected float sunsetSpeedMultiplier = 0.2f;
        [SerializeField]
        protected float nightSpeedMultiplier = 10.0f;

        float sunSpeedPct = 0.0f;

        float minAltitude = 0.1f;
        float maxAltitude = 100.0f;
        float altitudeSpeed = 30.0f;
        float altitudeSpeedPct = 0.0f;
        float currentAltitude = 0.1f;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            float targetAltitudeSpeedPct = 0.0f;
            if (Input.GetKey(KeyCode.UpArrow))
                targetAltitudeSpeedPct += 1.0f;
            if (Input.GetKey(KeyCode.DownArrow))
                targetAltitudeSpeedPct -= 1.0f;

            float altitudeChange = Mathf.Lerp(altitudeSpeedPct, targetAltitudeSpeedPct, Time.deltaTime * 5.0f) * currentAltitude * altitudeSpeed;
            currentAltitude = Mathf.Clamp(currentAltitude + altitudeChange * Time.deltaTime, minAltitude, maxAltitude);
            sky.Altitude = currentAltitude;


            float targetSunSpeedPct = 0.0f;
            if (Input.GetKey(KeyCode.RightArrow))
                targetSunSpeedPct += 1.0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                targetSunSpeedPct -= 1.0f;
            
            sunSpeedPct = Mathf.Lerp(sunSpeedPct, targetSunSpeedPct, Time.deltaTime * 5.0f);

            float sunDeltaTime = Time.deltaTime * sunSpeedPct;

            float sunsetY = -0.03f; // TODO: Find actual horizon angle based on altitude & planetary radius.
            float sunSpeed = 1.0f;

            sunSpeed *= sky.SunDirection.y < sunsetY - 0.1f ? nightSpeedMultiplier : 1.0f;
            float distanceToSunset = Mathf.Clamp01(Mathf.Abs(sky.SunDirection.y - sunsetY));
            sunSpeed *= Mathf.Lerp(sunsetSpeedMultiplier, 1.0f, distanceToSunset);

            time = time.AddHours((sunDeltaTime) * sunSpeed * (60.0f / minutesPerHour));

            double sunAltitude;
            double sunAzimuth;
            SunPosition.CalculateSunPosition(time, latitude, longitude, out sunAltitude, out sunAzimuth);

            sky.SunDirection = CartesianCoords.SphericalToCartesian((float)sunAzimuth, (float)sunAltitude);
        }

    }
}