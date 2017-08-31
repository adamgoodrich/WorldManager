using System;
using System.Collections.Generic;
using UnityEngine;

namespace WAPI
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
    /// To receive events when values are changed implement your own listener via the 
    /// IWorldApiChangeHandler interface and then connect it up
    ///
    ///     WorldManager.Instance.AddListener(this);
    /// 
    /// To stop receiving events when values change disconnect your handler e.g.
    ///     WorldManager.Instance.RemoveListener(this);
    /// 
    /// To shut everything down and start it up again use WorldAPIActive e.g.
    ///    WorldManager.Instance.WorldAPIActive = false;
    /// 
    /// To use as global variables in shaders the general naming standard is 
    /// _WAPI_[PropertyName], however in many instances the data has been stored
    /// in vectors for efficient transfer to the GPU, so take a peek at the code to 
    /// get the naming. At the very least however all shader variables are prefixed 
    /// with _WAPI_.
    /// 
    /// </summary>
    public sealed class WorldManager : MonoBehaviour
    {
        #region Interfaces

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.ManagerActiveChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.GameTimeChanged;
                    RaiseEvent();
                }
            }
        }

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

        /// <summary>
        /// Set the decimal time eg 0.. 23.99:
        /// </summary>
        /// <param name="time"></param>
        public void SetDecimalTime(double time)
        {
            var ts = TimeSpan.FromHours(time);
            var gameTime = new DateTime(m_gameTime.Year, m_gameTime.Month, m_gameTime.Day, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            if (m_gameTime != gameTime)
            {
                m_gameTime = gameTime;
                m_changeMask |= WorldConstants.WorldChangeEvents.GameTimeChanged;
                RaiseEvent();
            }
        }

        #endregion

        #region Player Location, Sea Level, Lat / Lng, Scene sizes

        /// <summary>
        /// Player object - the player / object that holds the main game camera
        /// </summary>
        public GameObject PlayerObject
        {
            get { return m_playerObject; }
            set
            {
                if (m_playerObject != value)
                {
                    m_playerObject = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.PlayerChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SeaChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.LatLngChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.LatLngChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SceneMetricsChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SceneMetricsChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SceneMetricsChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.TempAndHumidityChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.TempAndHumidityChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Wind

        /// <summary>
        /// Get and set wind - x = WindDirection, y = WindSpeed, z = WindTurbulence
        /// </summary>
        public Vector4 Wind
        {
            get { return m_windData; }
            set
            {
                if (m_windData != value)
                {
                    m_windData = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.WindChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.WindChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.WindChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.WindChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Fog

        /// <summary>
        /// Get and set fog, x = FogHeightPower, y = FogHeightMax, z = FogDistancePower, w = FogDistanceMax
        /// </summary>
        public Vector4 Fog
        { 
            get { return m_fogData; }
            set
            {
                if (m_fogData != value)
                {
                    m_fogData = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.FogChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Fog height power - in range 0 .. 1
        /// </summary>
        public float FogHeightPower
        {
            get { return m_fogData.x; }
            set
            {
                if (m_fogData.x != value)
                {
                    m_fogData.x = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.FogChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Maximum height for height based fog in world units / meters
        /// </summary>
        public float FogHeightMax
        {
            get { return m_fogData.y; }
            set
            {
                if (m_fogData.y != value)
                {
                    m_fogData.y = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.FogChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Fog distance power 0..1
        /// 
        /// </summary>
        public float FogDistancePower
        {
            get { return m_fogData.z; }
            set
            {
                if (m_fogData.z != value)
                {
                    m_fogData.z = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.FogChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Maximum fog distance in world units / meters
        /// </summary>
        public float FogDistanceMax
        {
            get { return m_fogData.w; }
            set
            {
                if (m_fogData.w != value)
                {
                    m_fogData.w = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.FogChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Rain

        /// <summary>
        /// Get and set rain - x = RainPower, y = RainPowerOnTerrain, z = RainMinHeight, w = RainMaxHeight
        /// </summary>
        public Vector4 Rain
        {
            get { return m_rainData; }
            set
            {
                if (m_rainData != value)
                {
                    m_rainData = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.RainChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.RainChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Rain power on terrain - in range 0 .. 1
        /// </summary>
        public float RainPowerTerrain
        {
            get { return m_rainData.y; }
            set
            {
                if (m_rainData.y != value)
                {
                    m_rainData.y = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.RainChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Minimum rain height in world units / meters
        /// </summary>
        public float RainMinHeight
        {
            get { return m_rainData.z; }
            set
            {
                if (m_rainData.z != value)
                {
                    m_rainData.z = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.RainChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Maximum rain height in world units / meters
        /// </summary>
        public float RainMaxHeight
        {
            get { return m_rainData.w; }
            set
            {
                if (m_rainData.w != value)
                {
                    m_rainData.w = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.RainChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Hail

        /// <summary>
        /// Get and set hail- x = HailPower, y = HailPowerOnTerrain, z = MinHailHeight, w = MaximumHailHeight
        /// </summary>
        public Vector4 Hail
        {
            get { return m_haildata; }
            set
            {
                if (m_haildata != value)
                {
                    m_haildata = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.HailChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.HailChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Hail power on terrain 0..1
        /// </summary>
        public float HailPowerTerrain
        {
            get { return m_haildata.y; }
            set
            {
                if (m_haildata.y != value)
                {
                    m_haildata.y = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.HailChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Minimum hail height in world units / meters
        /// </summary>
        public float HailMinHeight
        {
            get { return m_haildata.z; }
            set
            {
                if (m_haildata.z != value)
                {
                    m_haildata.z = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.HailChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Maximum hail height in world units / meters
        /// </summary>
        public float HailMaxHeight
        {
            get { return m_haildata.w; }
            set
            {
                if (m_haildata.w != value)
                {
                    m_haildata.w = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.HailChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Snow

        /// <summary>
        /// Get and set all snow, x = SnowPower, y = SnowPowerOnTerrain, z = SnowMinHeight, w = SnowAge
        /// </summary>
        public Vector4 Snow
        {
            get { return m_snowData; }
            set
            {
                if (m_snowData != value)
                {
                    m_snowData = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.SnowChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SnowChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Snow power on terrain in range 0..1
        /// </summary>
        public float SnowPowerTerrain
        {
            get { return m_snowData.y; }
            set
            {
                if (m_snowData.y != value)
                {
                    m_snowData.y = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.SnowChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Minimum snow height in world units / meters
        /// </summary>
        public float SnowMinHeight
        {
            get { return m_snowData.z; }
            set
            {
                if (m_snowData.z != value)
                {
                    m_snowData.z = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.SnowChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SnowChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.ThunderChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Clouds

        /// <summary>
        /// Get and set clouds - x = CloudPower, y = CloudMinHeight, z = CloudMaxHeight, w = CloudSpeed
        /// </summary>
        public Vector4 Clouds
        {
            get { return m_cloudData; }
            set
            {
                if (m_cloudData != value)
                {
                    m_cloudData = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.CloudsChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.CloudsChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.CloudsChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Maximim cloud height in world units / meters
        /// </summary>
        public float CloudMaxHeight
        {
            get { return m_cloudData.z; }
            set
            {
                if (m_cloudData.z != value)
                {
                    m_cloudData.z = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.CloudsChanged;
                    RaiseEvent();
                }
            }
        }

        /// <summary>
        /// Speed the clouds are travelling at in meters /sec
        /// </summary>
        public float CloudSpeed
        {
            get { return m_cloudData.w; }
            set
            {
                if (m_cloudData.w != value)
                {
                    m_cloudData.w = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.CloudsChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.MoonChanged;
                }
                RaiseEvent();
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.SeasonChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Sound levels

        /// <summary>
        /// Sound volumes, x = VolumeEnvironment, y = VolumeNPC, z = VolumeAnimals, w = VolumeWeather
        /// </summary>
        public Vector4 Volume
        {
            get { return m_soundVolumes; }
            set
            {
                if (m_soundVolumes != value)
                {
                    m_soundVolumes = value;
                    m_changeMask |= WorldConstants.WorldChangeEvents.VolumeChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.VolumeChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.VolumeChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.VolumeChanged;
                    RaiseEvent();
                }
            }
        }

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
                    m_changeMask |= WorldConstants.WorldChangeEvents.VolumeChanged;
                    RaiseEvent();
                }
            }
        }

        #endregion

        #region Extensions

        /// <summary>
        /// Extension data - store whatever you want here - if there is something in here thats not yours 
        /// then dont touch it, as it will depend on this in some way.
        /// </summary>
        public List<WorldManagerDataExtension> Extensions
        {
            get { return m_extensionList; }
            set { m_extensionList = value; }
        }

        #endregion

        #region Serialisation / Deserialisation

        /// <summary>
        /// Serialise the content of world manager into a JSON string
        /// </summary>
        /// <returns>JSON serialised string containing current state of world manager</returns>
        public string Serialise()
        {
            return JsonUtility.ToJson(this);
        }

        /// <summary>
        /// Deserialise the content of the world manager from the supplied string
        /// </summary>
        /// <param name="jsonState">State to be loaded into the manager</param>
        public void DeSerialise(string jsonState)
        {
            JsonUtility.FromJsonOverwrite(jsonState, this);
        }

        #endregion

        #endregion

        #region Unity Update and LateUpdate methods

        /// <summary>
        /// Execute extension updates
        /// </summary>
        void Update()
        {
            //Exit if not active
            if (!m_worldAPIActive)
            {
                return;
            }

            //Call update on extensions
            for (int idx = 0; idx < m_extensionList.Count; idx++)
            {
                m_extensionList[idx].Update();
            }
        }

        /// <summary>
        /// Send accumulated changes to shader variables at the end of update cycle, 
        /// executes extension late updates, and then clears the changed mask.
        /// </summary>
        void LateUpdate()
        {
            //Exit if we are not active
            if (!m_worldAPIActive)
            {
                return;
            }

            //Only do something if something changed
            if (m_changeMask != 0)
            {
                //This seems heavy - perhaps faster to add additional testing
                if ((m_changeMask & WorldConstants.WorldChangeEvents.ManagerActiveChanged) != 0)
                {
                    Shader.SetGlobalInt("_WAPI_WorldAPIActive", m_worldAPIActive ? 1 : 0);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.GameTimeChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_GameTime",
                        new Vector4(m_gameTime.Year, m_gameTime.Month, m_gameTime.Day, (float) GetTimeDecimal()));
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.PlayerChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_PlayerPosition", m_playerObject.transform.position);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.SeaChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Sea", m_seaData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.LatLngChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_LatLon", m_latLon);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.SceneMetricsChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_SceneGroundCenter", m_sceneGroundCenter);
                    Shader.SetGlobalVector("_WAPI_SceneCenter", m_sceneCenter);
                    Shader.SetGlobalVector("_WAPI_SceneSize", m_sceneSize);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.TempAndHumidityChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_TempHumid", m_tempAndHumidity);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.WindChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Wind", m_windData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.FogChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Fog", m_fogData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.RainChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Rain", m_rainData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.HailChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Hail", m_haildata);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.SnowChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Snow", m_snowData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.ThunderChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Thunder", m_thunderData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.CloudsChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Clouds", m_cloudData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.MoonChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Moon", m_moonData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.SeasonChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Season", m_seasonData);
                }
                if ((m_changeMask & WorldConstants.WorldChangeEvents.VolumeChanged) != 0)
                {
                    Shader.SetGlobalVector("_WAPI_Sound", m_soundVolumes);
                }

                //Flag all changes as done
                m_changeMask = 0;
            }

            //Give extensions a crack as well
            for (int idx = 0; idx < m_extensionList.Count; idx++)
            {
                m_extensionList[idx].LateUpdate();
            }
        }

        #endregion

        #region Event management

        private UInt64 m_changeMask = 0;
        private List<IWorldApiChangeHandler> m_listeners = new List<IWorldApiChangeHandler>();

        /// <summary>
        /// Add your listener
        /// </summary>
        /// <param name="listener">The listener to be added</param>
        public void AddListener(IWorldApiChangeHandler listener)
        {
            //Debug.Log("Adding listener " + listener.GetType());

            //Remove it if it was there
            m_listeners.Remove(listener);

            //And add it
            m_listeners.Add(listener);
        }

        /// <summary>
        /// Remove your listener
        /// </summary>
        /// <param name="listener">The listener to be removed</param>
        public void RemoveListener(IWorldApiChangeHandler listener)
        {
            m_listeners.Remove(listener);
        }

        /// <summary>
        /// Raise event if something has changed and world manager is active
        /// </summary>
        void RaiseEvent()
        {
            //Exit if we are not active
            if (!m_worldAPIActive)
            {
                return;
            }

            //Exit if nothing changed
            if (m_changeMask == 0)
            {
                return;
            }

            // Go backwards in list in case someone removes a listener
            for (var n = m_listeners.Count - 1; n >= 0; --n)
            {
                var listener = m_listeners[n];

                //Remove it if it no longer exists
                if (listener == null)
                {
                    m_listeners.RemoveAt(n);
                    continue;
                }

                //Send the message, trap errors, remove listener if it generates errots
                try
                {
                    listener.OnWorldChanged(new WorldChangeArgs(m_changeMask, this));
                }
                catch (System.Exception e)
                {
                    //Tell world about it
                    Debug.LogException(e, this);

                    //Remove the listener that causes the exception so remaining stuff continues to work
                    Debug.LogError("Removed listener because it caused error");
                    m_listeners.RemoveAt(n);
                }
            }
        }

        #endregion

        #region Serialised data - in vector4's where possible to make it easier to send to shaders

        //World API active
        [SerializeField]
        private bool m_worldAPIActive = true;

        //Game time
        [SerializeField]
        private DateTime m_gameTime;

        //Player object
        [SerializeField]
        private GameObject m_playerObject;

        //Latitude & longitude
        [SerializeField]
        private Vector4 m_latLon = new Vector4();

        //Ground level, center of scene, world units
        [SerializeField]
        private Vector3 m_sceneGroundCenter;

        //Center of scene, world units
        [SerializeField]
        private Vector3 m_sceneCenter;

        //Size of the scene in world units
        [SerializeField]
        private Vector3 m_sceneSize;

        //Sea level in world units
        [SerializeField]
        private Vector4 m_seaData;

        //Wind direction in degrees (0..360), speed in m/sec, turbulence
        [SerializeField]
        private Vector4 m_windData;

        //Temperature degrees centigrade, and humidity
        private Vector4 m_tempAndHumidity;

        //Cloud cover 0 none .. 1 full, cloud cover min height world units, cloud cover max height world units
        [SerializeField]
        private Vector4 m_cloudData;

        //Fog power, min height world units /m, max height world units /m
        [SerializeField]
        private Vector4 m_fogData;

        //Rain power, min height world units /m, max height world units /m
        [SerializeField]
        private Vector4 m_rainData;

        //Hail power, min height world units /m, max height world units /m
        [SerializeField]
        private Vector4 m_haildata;

        //Snow power 0..1, min height m, max height m, age 0..1
        [SerializeField]
        private Vector4 m_snowData;

        //Thunder power 0..1
        [SerializeField]
        private Vector4 m_thunderData;

        //Ambience volumes - global, npcs, animals, weather
        [SerializeField]
        private Vector4 m_soundVolumes = new Vector4(1f, 1f, 1f, 1f);

        //Season 0..4
        [SerializeField]
        private Vector4 m_seasonData;

        //Moon phase 0 none .. 1 full
        [SerializeField]
        private Vector4 m_moonData;

        //Extension data - placeholder for anything else you may want to store with world manager
        [SerializeField]
        private List<WorldManagerDataExtension> m_extensionList = new List<WorldManagerDataExtension>();
        #endregion

        #region Singleton & general class management

        //Stop people from instantiating directly because its a singleton
        private WorldManager() {}

        //This is a singleton - here is the instance
        private static WorldManager m_instance;

        //Access to the world manager instance
        public static WorldManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (WorldManager)FindObjectOfType(typeof(WorldManager));
                    if (m_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        singleton.hideFlags = HideFlags.HideAndDontSave;
                        singleton.name = typeof(WorldManager).ToString();
                        m_instance = singleton.AddComponent<WorldManager>();
                    }
                }
                return m_instance;
            }
        }

        #endregion
    }
}