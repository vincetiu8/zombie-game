# Networking guide

This game uses PUN 2 to connect the clients. We initially used Mirror, but the main issue with that was only the parent
object could have networking scripts, which is quite clunky in our case. This is not a competitive game, and as such we
will not implement any anti-cheating methods.

## Key terminology

State: What the game currently looks like. The game state is the overarching situation the players are currently in.
Each component has its own state in the game, which composes of its attributes. These include the object's position and
important variables in its scripts.

Sync(ronization): Ensuring the game state is the same in all clients. From any client's perspective, the game should
look the same and they should be able to perform the same actions.

Authority: Who controls an object. The client with authority sends the state of the object to all other clients, who
read and update their own internal state accordingly.

Master client: In Photon, the master client handles all the important calculations that need to be synchronized. They
also has authority over all objects aside from the players.

## Overview

When you're implementing a new feature, you need to ensure it's synchronized across all clients. Photon offers several
ways to do this, and the method you should take depends on what exactly you are trying to do.

## Photon view

In order to synchronize their state, all networked objects need to have a `Photon View` component in their parent
GameObject. This has multiple settings, but the defaults are normally acceptable.

## Syncing transforms

Example: syncing positions

To sync an object's position, rotation or scale, simply add the `Photon Transform View` to the object and select the
relevant attributes to sync.

## Syncing scripts

In order for a script to be synced, it needs to derive from `MonoBehaviourPun` instead of `MonoBehaviour`. This gives it
access to the `photonView` component attached to the object, among other functionality.

### Syncing variables

Example: syncing health

To sync a variable in a script, it needs to implement the `IPunObservable` interface. You need to add a method
called `OnPhotonSerializeView`, and then synchronize relevant variables inside. For example, if we want to synchronize
the player's health, we would write something like:

```
public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
{
	// If we are writing, we have authority over the object and dictate it's state
	if (stream.IsWriting) 
	{
		// Send the variable to the other clients
		stream.SendNext(health); 
		return;
	}

	// Receieve the variable and update our own state accordingly
	// Note the typecast: incoming variables from the stream are all objects
	// So we need to typecast them back to their original form
	health = (int) stream.ReceiveNext(); 
}
```

This approach is preferred for single variables that change often. If you are trying to sync something less often or a
larger struct, please use and RPC, as detailed below.

### Remote procedure calls (RPCs)

If we are going to synchronize something more complex, like the contents of a class, or need to call a method on another
client, we need to use an RPC. An RPC is a method that can be called by other clients, and normally we just want to call
it on all clients.

To label a method as an RPC, add the `[PunRPC]` tag to the method. The RPC will execute the method locally, modifying
the object's state. In order to call an RPC, we invoke `photonView.RPC`. Note that RPCs can only be called on the same
object/script, so you can't call RPCs on other objects.

Calling a method on all clients can be done by simply adding a RPC method to the script and specifying all clients as
the target when calling the RPC. As long as the object exists on the client, it will be updated accordingly. For
example, to destroy an object after death on all clients, we could write something like:

```
protected virtual void OnDeath()
{
	photonView.RPC("RpcOnDeath", RpcTarget.All);
}

[PunRPC]
protected void RpcOnDeath()
{
	Destroy(gameObject);
}
```

#### Shops and gold

All gold interactions are handled by the `GoldManager` script. We trust clients to be honest with their gold
calculations. To add, withdraw or get the player's gold, simply call the relevant methods in the `GoldManager`.

## Instantiation

To instantiate an object across all clients, use `PhotonNetwork.Instantiate`, passing in the name of the Object instead
of a Prefab reference. The resulting GameObject can then be accessed like normal. Note that the Prefab must be located
in a folder called `Resources`, and all non-networked prefabs should be in other folders.