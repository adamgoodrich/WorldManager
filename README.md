# WorldManager (WAPI)
World Manager (WAPI) is a generic world and environment command and control system for Unity that enables environmental assets to play nicely together.

It does this by providing a simple but powerful interface that allows you to control the components that render your environment a generic and coordinated way.

The intention is to bridge the gap between assets and make it easier to develop games by plugging them together using a common API. You will still configure your assets individually to your own taste, but coordination and control is generic and centralised.

Assets can choose which parts of the API they implement and regardless of how much they support, you will get value from this system because of the way it coordinates their behaviour.

Additionally, WAPI introduces a global set of shader variables, so that any shader that implements WAPI will automatically be updated when settings change without requiring the overhead associated with the subscription mechanism.

WAPI is free for you to use any way you like. It will be professionally maintained, and will also be packaged up as a free unity asset (soon). For more information on WAPI go to http://www.procedural-worlds.com/blog/wapi/.

WAPI was created by Adam Goodrich, the author of Gaia, GeNa, CTS (The Complete PBR Terrain Shader) and Pegasus. Adam is a specialist in environmental and procedural generation in Unity3D and this project was born out of his frustration with getting things to work nicely together. If you would like access to his free monthly newsletter with tips, tricks, reviews and freebies then sign up on his home page at http://www.procedural-worlds.com/.

## Basic Usage Pattern

To enable and disable all events, update and lateupdate
    WorldManager.Instance.WorldAPIActive = true/false;

To generate events, and get and set values use the following generic format e.g.
    WorldManager.Instance.Api();
    var value = WorldManager.Instance.Property;
    WorldManager.Instance.Property = value;

To receive events when values are changed implement your own listener via the IWorldApiChangeHandler interface and then connect it up e.g.

    public class WorldController : MonoBehaviour, IWorldApiChangeHandler
    {
        .. your stuff..

        void Start()
        {
            ConnectToWorldAPI();
        }

        //Let world manager API know you want to handle events
        void ConnectToWorldAPI()
        {
            WorldManager.Instance.AddListener(this);
        }

        //Let world manager API know that you are no longer interested
        void DisconnectFromWorldAPI()
        {
            WorldManager.Instance.RemoveListener(this);
        }

        //If this object has been added as a listener then it will be called whenever an event is fired,
        //use the changeArgs.HasChanged method to filter for the events you are interested in
        public void OnWorldChanged(WorldChangeArgs changeArgs)
        {
            if (changeArgs.HasChanged(WorldConstants.WorldChangeEvents.GameTimeChanged))
            {
                //Grab game time
                m_timeNow = (float)changeArgs.manager.GetTimeDecimal();

                //Do whatever logic you want
                m_timeNow += 0.25f;

                //Set it back into world manager -> NOTE you would never do THIS SPECIFIC THING
                //as this will cause another OnWorldChanged event to be generated, which would in turn
                //cause this to be executed again in one nasty loop
                WorldManager.Instance.SetDecimalTime(m_timeNow);
            }
        }
    }

Take a look at WorldController.cs for a simple example of how to use the API. It both listens to things
and also implements a simple user interface that controls it in the editor and at runtime.

To use as global variables in shaders the general naming standard is \_WAPI\_[PropertyName], however in many instances the data has been stored in vectors for efficient transfer to the GPU, so take a peek at the code to get the naming. At the very least however all shader variables are prefixed with \_WAPI\_.

## API Categories

* IsActive
* Time
* Player Location, Sea Level, Latitude & Longitude
* Temperature and humidity
* Wind
* Fog
* Rain
* Hail
* Snow
* Thunder
* Clouds
* Moon
* Season
* Sound Levels
* Extensions
* Serialisation

## Serialisation system

The serialisation system allows you to manage the process of saving and loading WAPI data. The choice of when to do this and how to store the serialised data are up to you.

## Extension system

The extension system allows you to create your own extension sub classes and add them to WAPI to be managed. You could add additional game state information as well handlers for Update and LateUpdate and have them called on your extensions when World Manager updates. Just derive your class from WorldManagerExtension, and add it to the extensions list.
