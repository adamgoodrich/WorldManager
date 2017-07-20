# WorldManager
A generic world and environment API for Unity to enable environmental assets to play nicely together.

WorldManager allows you to control the components that render your environment in a generic and coordinated way. 

The intention is to bridge the gap between assets and make it easier to develop games by plugging them together using a common API. You will still configure assets individually to your own taste, but coordination and control is generic and centralised.

Assets can choose which parts of the API they implement and regardless of how much they support, you will still get  value from this system because of the way it coordinates theeir behaviour.

To generate events, and get and set values use the following generic format e.g. 
    WorldManager.Instance.Api()

To receive events when values are changed connect up the event handlers e.g. 
    WorldManager.Instance.OnApiChangedHandler += YourHandler()

To stop receiving events when values change disconnect your handler e.g.
    WorldManager.Instance.OnApiChangedHandler -= YourHandler()
