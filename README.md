# WorldManager (WAPI)
World Manager (WAPI) is a generic world and environment command and control system for Unity that enables environmental assets to play nicely together.

It does this be providing a simple but powerful interface that allows you to control the components that render your environment a generic and coordinated way.

The intention is to bridge the gap between assets and make it easier to develop games by plugging them together using a common API. You will still configure your assets individually to your own taste, but coordination and control is generic and centralised.

Assets can choose which parts of the API they implement and regardless of how much they support, you will get value from this system because of the way it coordinates their behaviour.

Additionally, WAPI introduces a global set of shader variables, so that any shader that implements WAPI will automatically be updated when settings change without requiring the overhead associated with the subscription mechanism.

This API will be kept stable and professionaly maintained, and will also be available as a free unity asset (soon). For more information go to https://forum.unity3d.com/threads/world-manager-generic-world-management-system.484239/.

## Basic Usage Pattern

To generate events, and get and set values use the following generic format e.g. 
    WorldManager.Instance.Api()

To receive events when values are changed connect up the event handlers e.g. 
    WorldManager.Instance.OnApiChangedHandler += YourHandler()

To stop receiving events when values change disconnect your handler e.g.
    WorldManager.Instance.OnApiChangedHandler -= YourHandler()

To use as global variables in shaders the general naming standard is \\_WAPI\\_[PropertyName], however in many instances the data has been stored in vectors for efficient transfer to the GPU, so take a peek at the code to get the naming. At the very least however all shader variables are prefixed with \\_WAPI\\_.

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