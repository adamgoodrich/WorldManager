using System;
using UnityEngine;

namespace WorldAPI
{
    /// <summary>
    /// 
    /// WorldManager API (WAPI) allows you to control the components that render your 
    /// environment a generic and coordinated way. 
    /// 
    /// The intention is to bridge the gap between assets and make it easier to 
    /// develop games by plugging them together using a common API. You will still
    /// configure assets individually to your own taste, but coordination and control 
    /// is generic and centralised.
    /// 
    /// Assets can choose which parts of the API they implement and regardless of how 
    /// much they support, you will still get  value from this system because of the 
    /// way it coordinates their behaviour.
    /// 
    /// To generate events, and get and set values use the following generic format e.g. 
    ///     WorldManager.Instance.Api()
    /// 
    /// To receive events when values are changed connect up the event handlers e.g. 
    ///     WorldManager.Instance.OnApiChangedHandler += YourHandler()
    /// 
    /// To stop receiving events when values change disconnect your handler e.g.
    ///     WorldManager.Instance.OnApiChangedHandler -= YourHandler()
    /// 
    /// To use as global variables in shaders the general naming standard is 
    /// _WAPI_[PropertyName], however in many instances the data has been stored
    /// in vectors for efficient transfer to the GPU, so take a peek at the code to 
    /// get the naming. At the very least however all shader variables are prefixed 
    /// with _WAPI_.
    /// 
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        #region External interfaces

        #region IsActive 

        /// <summary>
        /// Whether or not the world api is active
        /// </summary>
        public bool WorldAPIActive
        {
            get { return m_worldAPIActive; }
            set
            {
                if (m_worldAPIActive != value)
                {
                    m_worldAPIActive = value;
                    Shader.SetGlobalInt("_WAPI_WorldAPIActive", m_worldAPIActive ? 1 : 0);
                    if (OnWorldAPIActiveChanged != null)
                    {
                        OnWorldAPIActiveChanged(this, m_worldAPIActive);
                    }
                }
            }
        }
        public event BoolChangedEventHandler OnWorldAPIActiveChanged;

        #endregion

        #region Time

        /// <summary>
        /// Current game date and time
        /// </summary>
        public DateTime GameTime
        {
            get { return m_gameTime; }
            set
            {
                if (m_gameTime != value)
                {
                    m_gameTime = value;
                    Shader.SetGlobalVector("_WAPI_GameTime", new Vector4(m_gameTime.Year, m_gameTime.Month, m_gameTime.Day, (float)GetTimeDecimal()));
                    if (OnGameTimeChanged != null)
                    {
                        OnGameTimeChanged(this, m_gameTime);
                    }
                }
            }
        }
        public event TimeChangedEventHandler OnGameTimeChanged;

        /// <summary>
        /// Get time in scientific decimal format accurate to millisecond  0.0000 .. 23.9999 ie hours.minutessecsmillisecs
        /// </summary>
        /// <returns>Game time as a decimal value</returns>
        public double GetTimeDecimal()
        {
            return ((double)m_gameTime.Hour) + 
                TimeSpan.FromMinutes(m_gameTime.Minute).TotalHours + 
                TimeSpan.FromSeconds(m_gameTime.Second).TotalHours +
                TimeSpan.FromMilliseconds(m_gameTime.Millisecond).TotalHours;
        }

        #endregion

        #region Player Location, Sea Level, Lat / Lng, Scene sizes

        /// <summary>
        /// Player location world units - useful as a lot of FX require a location to work effectively
        /// </summary>
        public Vector3 PlayerLocation
        {
            get { return m_playerLocation; }
            set
            {
                if (m_playerLocation != value)
                {
                    m_playerLocation = value;
                    Shader.SetGlobalVector("_WAPI_PlayerLocation", m_playerLocation);
                    if (OnPlayerLocationChanged != null)
                    {
                        OnPlayerLocationChanged(this, m_playerLocation);
                    }
                }
            }
        }
        public event Vector3ChangedEventHandler OnPlayerLocationChanged;

        /// <summary>
        /// Sea Level - world units
        /// </summary>
        public float SeaLevel
        {
            get { return m_seaData.x; }
            set
            {
                if (m_seaData.x != value)
                {
                    m_seaData.x = value;
                    Shader.SetGlobalVector("_WAPI_Sea", m_seaData);
                    if (OnSeaLevelChanged != null)
                    {
                        OnSeaLevelChanged(this, m_seaData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnSeaLevelChanged;

        /// <summary>
        /// Latitude
        /// </summary>
        public float Latitude
        {
            get { return m_latLon.x; }
            set
            {
                if (m_latLon.x != value)
                {
                    m_latLon.x = value;
                    Shader.SetGlobalVector("_WAPI_LatLon", m_latLon);
                    if (OnLatitudeChanged != null)
                    {
                        OnLatitudeChanged(this, m_latLon.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnLatitudeChanged;

        /// <summary>
        /// Longitude
        /// </summary>
        public float Longitude
        {
            get { return m_latLon.y; }
            set
            {
                if (m_latLon.y != value)
                {
                    m_latLon.y = value;
                    Shader.SetGlobalVector("_WAPI_LatLon", m_latLon);
                    if (OnLongitudeChanged != null)
                    {
                        OnLongitudeChanged(this, m_latLon.y);
                    }
                }                
            }
        }
        public event FloatChangedEventHandler OnLongitudeChanged;

        /// <summary>
        /// Ground level at center of scene in world units - useful for placement
        /// </summary>
        public Vector3 SceneGroundCenter
        {
            get { return m_sceneGroundCenter; }
            set
            {
                if (m_sceneGroundCenter != value)
                {
                    m_sceneGroundCenter = value;
                    Shader.SetGlobalVector("_WAPI_SceneGroundCenter", m_sceneGroundCenter);
                    if (OnSceneGroundCenterChanged != null)
                    {
                        OnSceneGroundCenterChanged(this, m_sceneGroundCenter);
                    }
                }
            }
        }
        public event Vector3ChangedEventHandler OnSceneGroundCenterChanged;

        /// <summary>
        /// Center of scene in world units - useful for placement
        /// </summary>
        public Vector3 SceneCenter
        {
            get { return m_sceneCenter; }
            set
            {
                if (m_sceneCenter != value)
                {
                    m_sceneCenter = value;
                    Shader.SetGlobalVector("_WAPI_SceneCenter", m_sceneCenter);
                    if (OnSceneCenterChanged != null)
                    {
                        OnSceneCenterChanged(this, m_sceneCenter);
                    }
                }
            }
        }
        public event Vector3ChangedEventHandler OnSceneCenterChanged;

        /// <summary>
        /// Size of the scene in world units - useful for placement
        /// </summary>
        public Vector3 SceneSize
        {
            get { return m_sceneSize; }
            set
            {
                if (m_sceneSize != value)
                {
                    m_sceneSize = value;
                    Shader.SetGlobalVector("_WAPI_SceneSize", m_sceneSize);
                    if (OnSceneSizeChanged != null)
                    {
                        OnSceneSizeChanged(this, m_sceneSize);
                    }
                }
            }
        }
        public event Vector3ChangedEventHandler OnSceneSizeChanged;

        #endregion

        #region Temperature and humidity

        /// <summary>
        /// Temperature degrees centigrade
        /// </summary>
        public float Temperature
        {
            get { return m_tempAndHumidity.x; }
            set
            {
                if (m_tempAndHumidity.x != value)
                {
                    m_tempAndHumidity.x = value;
                    Shader.SetGlobalVector("_WAPI_TempHumid", m_tempAndHumidity);
                    if (OnTemperatureChanged != null)
                    {
                        OnTemperatureChanged(this, m_tempAndHumidity.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnTemperatureChanged;

        /// <summary>
        /// Humidity
        /// </summary>
        public float Humidity
        {
            get { return m_tempAndHumidity.y; }
            set
            {
                if (m_tempAndHumidity.y != value)
                {
                    m_tempAndHumidity.y = value;
                    Shader.SetGlobalVector("_WAPI_TempHumid", m_tempAndHumidity);
                    if (OnHumidityChanged != null)
                    {
                        OnHumidityChanged(this, m_tempAndHumidity.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnHumidityChanged;

        #endregion

        #region Wind

        /// <summary>
        /// Wind direction 0..360f
        /// </summary>
        public float WindDirection
        {
            get { return m_windData.x; }
            set
            {
                if (m_windData.x != (value % 360f))
                {
                    m_windData.x = value % 360f;
                    Shader.SetGlobalVector("_WAPI_Wind", m_windData);
                    if (OnWindDirectionChanged != null)
                    {
                        OnWindDirectionChanged(this, m_windData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnWindDirectionChanged;

        /// <summary>
        /// Wind speed m/sec
        /// </summary>
        public float WindSpeed
        {
            get { return m_windData.y; }
            set
            {
                if (m_windData.y != value)
                {
                    m_windData.y = value;
                    Shader.SetGlobalVector("_WAPI_Wind", m_windData);
                    if (OnWindSpeedChanged != null)
                    {
                        OnWindSpeedChanged(this, m_windData.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnWindSpeedChanged;

        /// <summary>
        /// Wind turbulence
        /// </summary>
        public float WindTurbulence
        {
            get { return m_windData.z; }
            set
            {
                if (m_windData.z != value)
                {
                    m_windData.z = value;
                    Shader.SetGlobalVector("_WAPI_Wind", m_windData);
                    if (OnWindTurbulenceChanged != null)
                    {
                        OnWindTurbulenceChanged(this, m_windData.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnWindTurbulenceChanged;

        #endregion

        #region Fog

        /// <summary>
        /// Fog power - in range 0 .. 1
        /// </summary>
        public float FogPower
        {
            get { return m_fogData.x; }
            set
            {
                if (m_fogData.x != value)
                {
                    m_fogData.x = value;
                    Shader.SetGlobalVector("_WAPI_Fog", m_fogData);
                    if (OnFogPowerChanged != null)
                    {
                        OnFogPowerChanged(this, m_fogData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnFogPowerChanged;

        /// <summary>
        /// Minimum fog height in world units / meters
        /// </summary>
        public float FogMinHeight
        {
            get { return m_fogData.y; }
            set
            {
                if (m_fogData.y != value)
                {
                    m_fogData.y = value;
                    Shader.SetGlobalVector("_WAPI_Fog", m_fogData);
                    if (OnFogMinHeightChanged != null)
                    {
                        OnFogMinHeightChanged(this, m_fogData.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnFogMinHeightChanged;

        /// <summary>
        /// Maximum fog height in world units / meters
        /// </summary>
        public float FogMaxHeight
        {
            get { return m_fogData.z; }
            set
            {
                if (m_fogData.z != value)
                {
                    m_fogData.z = value;
                    Shader.SetGlobalVector("_WAPI_Fog", m_fogData);
                    if (OnFogMaxHeightChanged != null)
                    {
                        OnFogMaxHeightChanged(this, m_fogData.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnFogMaxHeightChanged;

        #endregion

        #region Rain

        /// <summary>
        /// Rain power - in range 0 .. 1
        /// </summary>
        public float RainPower
        {
            get { return m_rainData.x; }
            set
            {
                if (m_rainData.x != value)
                {
                    m_rainData.x = value;
                    Shader.SetGlobalVector("_WAPI_Rain", m_rainData);
                    if (OnRainPowerChanged != null)
                    {
                        OnRainPowerChanged(this, m_rainData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnRainPowerChanged;

        /// <summary>
        /// Minimum rain height in world units / meters
        /// </summary>
        public float RainMinHeight
        {
            get { return m_rainData.y; }
            set
            {
                if (m_rainData.y != value)
                {
                    m_rainData.y = value;
                    Shader.SetGlobalVector("_WAPI_Rain", m_rainData);
                    if (OnRainMinHeightChanged != null)
                    {
                        OnRainMinHeightChanged(this, m_rainData.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnRainMinHeightChanged;

        /// <summary>
        /// Maximum rain height in world units / meters
        /// </summary>
        public float RainMaxHeight
        {
            get { return m_rainData.z; }
            set
            {
                if (m_rainData.z != value)
                {
                    m_rainData.z = value;
                    Shader.SetGlobalVector("_WAPI_Rain", m_rainData);
                    if (OnRainMaxHeightChanged != null)
                    {
                        OnRainMaxHeightChanged(this, m_rainData.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnRainMaxHeightChanged;

        #endregion

        #region Hail

        /// <summary>
        /// Hail power - in range 0 .. 1
        /// </summary>
        public float HailPower
        {
            get { return m_haildata.x; }
            set
            {
                if (m_haildata.x != value)
                {
                    m_haildata.x = value;
                    Shader.SetGlobalVector("_WAPI_Hail", m_haildata);
                    if (OnHailPowerChanged != null)
                    {
                        OnHailPowerChanged(this, m_haildata.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnHailPowerChanged;

        /// <summary>
        /// Minimum hail height in world units / meters
        /// </summary>
        public float HailMinHeight
        {
            get { return m_haildata.y; }
            set
            {
                if (m_haildata.y != value)
                {
                    m_haildata.y = value;
                    Shader.SetGlobalVector("_WAPI_Hail", m_haildata);
                    if (OnHailMinHeightChanged != null)
                    {
                        OnHailMinHeightChanged(this, m_haildata.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnHailMinHeightChanged;

        /// <summary>
        /// Maximum hail height in world units / meters
        /// </summary>
        public float HailMaxHeight
        {
            get { return m_haildata.z; }
            set
            {
                if (m_haildata.z != value)
                {
                    m_haildata.z = value;
                    Shader.SetGlobalVector("_WAPI_Hail", m_haildata);
                    if (OnHailMaxHeightChanged != null)
                    {
                        OnHailMaxHeightChanged(this, m_haildata.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnHailMaxHeightChanged;

        #endregion

        #region Snow

        /// <summary>
        /// Snow power - in range 0 .. 1
        /// </summary>
        public float SnowPower
        {
            get { return m_snowData.x; }
            set
            {
                if (m_snowData.x != value)
                {
                    m_snowData.x = value;
                    Shader.SetGlobalVector("_WAPI_Snow", m_snowData);
                    if (OnSnowPowerChanged != null)
                    {
                        OnSnowPowerChanged(this, m_snowData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnSnowPowerChanged;

        /// <summary>
        /// Minimum snow height in world units / meters
        /// </summary>
        public float SnowMinHeight
        {
            get { return m_snowData.y; }
            set
            {
                if (m_snowData.y != value)
                {
                    m_snowData.y = value;
                    Shader.SetGlobalVector("_WAPI_Snow", m_snowData);
                    if (OnSnowMinHeightChanged != null)
                    {
                        OnSnowMinHeightChanged(this, m_snowData.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnSnowMinHeightChanged;

        /// <summary>
        /// Maximum snow height in world units / meters
        /// </summary>
        public float SnowMaxHeight
        {
            get { return m_snowData.z; }
            set
            {
                if (m_snowData.z != value)
                {
                    m_snowData.z = value;
                    Shader.SetGlobalVector("_WAPI_Snow", m_snowData);
                    if (OnSnowMaxHeightChanged != null)
                    {
                        OnSnowMaxHeightChanged(this, m_snowData.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnSnowMaxHeightChanged;

        /// <summary>
        /// Snow age 0 fresh.. 1 old / crystalized
        /// </summary>
        public float SnowAge
        {
            get { return m_snowData.w; }
            set
            {
                if (m_snowData.w != value)
                {
                    m_snowData.w = value;
                    Shader.SetGlobalVector("_WAPI_Snow", m_snowData);
                    if (OnSnowAgeChanged != null)
                    {
                        OnSnowAgeChanged(this, m_snowData.w);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnSnowAgeChanged;

        #endregion

        #region Thunder

        /// <summary>
        /// Thunder power - in range 0 .. 1
        /// </summary>
        public float ThunderPower
        {
            get { return m_thunderData.x; }
            set
            {
                if (m_thunderData.x != value)
                {
                    m_thunderData.x = value;
                    Shader.SetGlobalVector("_WAPI_Thunder", m_thunderData);
                    if (OnThunderPowerChanged != null)
                    {
                        OnThunderPowerChanged(this, m_thunderData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnThunderPowerChanged;

        #endregion

        #region Clouds

        /// <summary>
        /// Cloud power - in range 0 .. 1
        /// </summary>
        public float CloudPower
        {
            get { return m_cloudData.x; }
            set
            {
                if (m_cloudData.x != value)
                {
                    m_cloudData.x = value;
                    Shader.SetGlobalVector("_WAPI_Clouds", m_cloudData);
                    if (OnCloudPowerChanged != null)
                    {
                        OnCloudPowerChanged(this, m_cloudData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnCloudPowerChanged;

        /// <summary>
        /// Minimum cloud height in world units / meters
        /// </summary>
        public float CloudMinHeight
        {
            get { return m_cloudData.y; }
            set
            {
                if (m_cloudData.y != value)
                {
                    m_cloudData.y = value;
                    Shader.SetGlobalVector("_WAPI_Clouds", m_cloudData);
                    if (OnCloudMinHeightChanged!= null)
                    {
                        OnCloudMinHeightChanged(this, m_cloudData.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnCloudMinHeightChanged;

        /// <summary>
        /// Maximum cloud height in world units / meters
        /// </summary>
        public float CloudMaxHeight
        {
            get { return m_cloudData.z; }
            set
            {
                if (m_cloudData.z != value)
                {
                    m_cloudData.z = value;
                    Shader.SetGlobalVector("_WAPI_Clouds", m_cloudData);
                    if (OnCloudMaxHeightChanged != null)
                    {
                        OnCloudMaxHeightChanged(this, m_cloudData.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnCloudMaxHeightChanged;

        #endregion

        #region Moon
        
        /// <summary>
        /// Moon phase 0 none .. 1 full
        /// </summary>
        public float MoonPhase
        {
            get { return m_moonData.x; }
            set
            {
                if (m_moonData.x != value)
                {
                    m_moonData.x = value;
                    Shader.SetGlobalVector("_WAPI_Moon", m_moonData);
                    if (OnMoonPhaseChanged != null)
                    {
                        OnMoonPhaseChanged(this, m_moonData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnMoonPhaseChanged;

        #endregion

        #region Season

        /// <summary>
        /// Season
        /// </summary>
        public float Season
        {
            get { return m_seasonData.x; }
            set
            {
                if (m_seasonData.x != value)
                {
                    m_seasonData.x = value;
                    Shader.SetGlobalVector("_WAPI_Season", m_seasonData);
                    if (OnSeasonChanged != null)
                    {
                        OnSeasonChanged(this, m_seasonData.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnSeasonChanged;

        #endregion

        #region Sound levels

        /// <summary>
        /// Global environment volume
        /// </summary>
        public float VolumeEnvironment
        {
            get { return m_soundVolumes.x; }
            set
            {
                if (m_soundVolumes.x != value)
                {
                    m_soundVolumes.x = value;
                    Shader.SetGlobalVector("_WAPI_Sound", m_soundVolumes);
                    if (OnVolumeEnvironmentChanged != null)
                    {
                        OnVolumeEnvironmentChanged(this, m_soundVolumes.x);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnVolumeEnvironmentChanged;

        /// <summary>
        /// NPC volume
        /// </summary>
        public float VolumeNPC
        {
            get { return m_soundVolumes.y; }
            set
            {
                if (m_soundVolumes.y != value)
                {
                    m_soundVolumes.y = value;
                    Shader.SetGlobalVector("_WAPI_Sound", m_soundVolumes);
                    if (OnNPCVolumeChanged != null)
                    {
                        OnNPCVolumeChanged(this, m_soundVolumes.y);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnNPCVolumeChanged;

        /// <summary>
        /// Animal Volume
        /// </summary>
        public float VolumeAnimals
        {
            get { return m_soundVolumes.z; }
            set
            {
                if (m_soundVolumes.z != value)
                {
                    m_soundVolumes.z = value;
                    Shader.SetGlobalVector("_WAPI_Sound", m_soundVolumes);
                    if (OnVolumeAnimalsChanged != null)
                    {
                        OnVolumeAnimalsChanged(this, m_soundVolumes.z);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnVolumeAnimalsChanged;

        /// <summary>
        /// Weather Volume
        /// </summary>
        public float VolumeWeather
        {
            get { return m_soundVolumes.w; }
            set
            {
                if (m_soundVolumes.w != value)
                {
                    m_soundVolumes.w = value;
                    Shader.SetGlobalVector("_WAPI_Sound", m_soundVolumes);
                    if (OnVolumeWeatherChanged != null)
                    {
                        OnVolumeWeatherChanged(this, m_soundVolumes.w);
                    }
                }
            }
        }
        public event FloatChangedEventHandler OnVolumeWeatherChanged;

        #endregion

        #endregion

        #region Serialised data - in vector4's where possible to make it easier to send to shaders

        //World API active
        [SerializeField]
        protected bool m_worldAPIActive = true;

        //Game time
        [SerializeField]
        protected DateTime m_gameTime;

        //Player location
        [SerializeField]
        protected Vector3 m_playerLocation;

        //Latitude & longitude
        [SerializeField]
        protected Vector4 m_latLon = new Vector4();

        //Ground level, center of scene, world units
        [SerializeField]
        protected Vector3 m_sceneGroundCenter;

        //Center of scene, world units
        [SerializeField]
        protected Vector3 m_sceneCenter;

        //Size of the scene in world units
        [SerializeField]
        protected Vector3 m_sceneSize;

        //Sea level in world units
        [SerializeField]
        protected Vector4 m_seaData;

        //Wind direction in degrees (0..360), speed in m/sec, turbulence
        [SerializeField]
        protected Vector4 m_windData;

        //Temperature degrees centigrade, and humidity
        protected Vector4 m_tempAndHumidity;

        //Cloud cover 0 none .. 1 full, cloud cover min height world units, cloud cover max height world units
        [SerializeField]
        protected Vector4 m_cloudData;

        //Fog power, min height world units /m, max height world units /m
        [SerializeField]
        protected Vector4 m_fogData;

        //Rain power, min height world units /m, max height world units /m
        [SerializeField]
        protected Vector4 m_rainData;

        //Hail power, min height world units /m, max height world units /m
        [SerializeField]
        protected Vector4 m_haildata;

        //Snow power 0..1, min height m, max height m, age 0..1
        [SerializeField]
        protected Vector4 m_snowData;

        //Thunder power 0..1
        [SerializeField]
        protected Vector4 m_thunderData;

        //Ambience volumes - global, npcs, animals, weather
        [SerializeField]
        protected Vector4 m_soundVolumes = new Vector4(1f, 1f, 1f, 1f);

        //Season 0..4
        [SerializeField]
        protected Vector4 m_seasonData;

        //Moon phase 0 none .. 1 full
        [SerializeField]
        protected Vector4 m_moonData;

        #endregion

        #region Delegates
        public delegate void BoolChangedEventHandler(WorldManager wm, bool newValue);
        public delegate void Vector3ChangedEventHandler(WorldManager wm, Vector3 newValue);
        public delegate void FloatChangedEventHandler(WorldManager wm, float newValue);
        public delegate void TimeChangedEventHandler(WorldManager wm, DateTime newValue);
        #endregion

        #region Singleton / class management

        //Stop people from instantiating directly
        protected WorldManager() {}

        //This is a singleton - here is the instance
        private static WorldManager m_instance;

        //Locking mechanism for access
        private static object m_lock = new object();

        //Access to the world manager instance
        public static WorldManager Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_instance == null)
                    {
                        m_instance = (WorldManager)FindObjectOfType(typeof(WorldManager));

                        if (FindObjectsOfType(typeof(WorldManager)).Length > 1)
                        {
                            return m_instance;
                        }

                        if (m_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            m_instance = singleton.AddComponent<WorldManager>();
                            singleton.name = typeof(WorldManager).ToString();
                            if (Application.isPlaying)
                            {
                                DontDestroyOnLoad(singleton);
                            }
                        }
                    }

                    return m_instance;
                }
            }
        }

        #endregion
    }
}