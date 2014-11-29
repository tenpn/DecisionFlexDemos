# DecisionFlex Demos {#demos}

These demos go hand-in-hand with the DecisionFlex library http://www.tenpn.com/decisionflex.html

They are made open-source under the MIT license so potential purchasers can view the samples first. They will not function standing alone, without the DecisionFlex core library too.

The *iPerson* scene to see a utility system set up in a sims-like situation. In the hierarchy view, under iPerson, see that there are Actions and Attributes. Attributes contains things like Fitness, Hunger, and Thirst. Actions contains the DecisionFlex, and its child actions are things like Drink, Eat and Exercise. 

Notice that the Eat action gameobject contains two child ModifyAttribute behaviours (derived from IAction). One decreases hunger, the other decreases fitness. Both are run when the Eat action is selected.

Every tick, the iPerson will decide on an action:
* First the decision maker will ask the AttributeContextFactory, which is a custom class derived from ContextFactory, to push all the attributes into a single ConsiderationContext object. 
* Then it will evaluate each child action object in turn. It will fetch all the Consideration objects from that Action, and pass the attribute context to each. Each will return a score. 
* All the scores of a action are multiplied together to reach a final score
* Once all actions are scored, SelectWeightedRandom is used to pick a semi-random choice. 

When you run the scene, you see a graph of attributes with taken actions labeled. You should see the iPerson behave moderately sensibly. 

The *WeaponSelection* scene contains a soldier object and a few enemies. DecisionFlex decides what weapon and target the soldier should use each frame. The soldier's WeaponSelection gameobject contains the DecisionFlex as well as Inventory, listing the current ammo reserves of the soldier.

It behaves similarly to the iPerson demo, except every frame the WeaponSelectionContextFactory returns multiple contexts, rather than just one. Each contains information about a single enemy, as well as soldier inventory status. Each weapon action is then scored multiple times, once per enemy, and the highest enemy/weapon combo is used. 

Notice that the chosen enemy context is passed to the weapon action, so it can correctly shoot.

When playing this scene, you can edit the soldier's ammo counts to see how this adjusts the actions, as well as pause the oscillating enemies. If running this in the editor, you can use the scene view to drag around the enemies and play with different scenarios. 

The *ufo* scene shows how to use DecisionFlex at scale. Here UFOs are spawned to orbit and attack a spaceship. As the spaceship whittles down the UFO health, the UFO may opt to orbit the green health station instead to recharge. If the UFO dies, a new UFO is instantiated.

The UFOs use \ref TenPN.DecisionFlex.TimesliceDecisionMaking "TimesliceDecisionMaking" to manage CPU load. If you have Unity Pro, you can profile the scene and see there's no allocations made during decision. 
