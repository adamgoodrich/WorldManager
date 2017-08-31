World Manager API (WAPI)
==================

Welcome to the World Manager API (WAPI).

World Manager API (WAPI) is a light weight command and control system for Unity 3D that enables assets to integrate nicely together via a simple API.

It bridges the gap between your game and the assets it uses so that systems that support it can be quickly and easily configured to work together – without coding!

Check out the DemoIntegration script in the Demo directory to see a sample integration. Also take a look at WorldController.cs for another sample integration.

USAGE:

To generate events, and get and set values use the following generic format e.g. WorldManager.Instance.ApiCall()

To receive events add the IWorldApiChangeHandler interface and implement OnWorldChanged and then listen for the messages you want to hear, or alternatively get trhe values direct from the World API methods you are interested in via the generic WorldManager.Instance.APIYourCalling();

To switch event delivery on and off use the AddListener and RemoveListener methods.


NOTE: When present in a project WAPI will inject the WORLDAPI_PRESENT symbol, so wrap your class in an #if ... #endif statement in order to avoid compilation errors if WAPI is missing in the target project.

To learn more about World Manager API head on over to:
http://www.procedural-worlds.com/blog/wapi/