This folder contains a PlantUml project of the source code.

# 2022-10-09
There has been some writings on the model. Finally I figured: "It's time to submit what I have". After working in the LinQ department it seems a bit unimaginable to figure out this model of Iso20022 eRepository, which is why we are trying to draw it. I can say it is not an easy task, because of the sheer size of that file and eventhough I am proud owner of LinQ scripts, which can generate a dumpable and collapsable model.
I finally managed to take some of the easy classes and draw them up, but when I moved over to the semi complex ones, it becomes even harder. That is due to the inheritance, which is invisible in the Xml model. I like abstractions just as much as the next, but looking through the data it is impossible to make sure that you get every needles in the haystack. Which is why I have decided to abandon this UML model until I can make a neat and queriable model in LinQ. That will help me locate all of the sweetspots, where I am missing some inheritance. That is also the key to find the correct way of making the abstractions in the generated model in the future. One further note - couldn't it be nice to actually be able to generate a PlantUml model alongside the actual classes?

# 2022-10-21
So finally there is generated a PlantUml file from LinQPad query. The funny part is that it is not previewable. To think of all the different possibilities that a specific type can be in: Missing a "MinOccur" field or a different kind of "DerivedComponent" fields or something like that. Even if we look at the list as distinctive, it is still too many components to make it PlantUml previewable. To top that one, the puml file generated is still too large to be easily trimmed down by hand (I should know).

# 2022-10-25
It finally happened! Something, which looks like a PlantUml model was generated. It is now linking the RawChildren and the XmlProperties to eachother. This will minimize the manual work needed afterwards. This means that the next step of understanding this weird model is in the PlantUml project - now we have the base code. Next step is to modify the base code to model the structure in some neat way.

# 2022-10-26
YAY! The model has been re-exported as SVG. It is still a mess, but I finally got it cleaned up a bit. It is still a mess, but now it is organized chaos instead of chaotic chaos. Adding some namespaces (which by the way is bad to do in a 600+ line PlantUml file - you might skip something and the namespacing vs. packages are not the same) with some colors and pseudo-organizing it gives a better overview. What's left now is actually checking the model through for errors to see what is missed.

One thing I can see (by looking into **iso20022_MessageAssociationEnd**) is that it is not complete. Those annoying traces and types are still an issue. So there is two things to do going forward here: Fix the generator or manually fix the model by walking through the forrest of Types, which <i>could be</i> an ID or a string (sigh).

I think the plan for now is to make a copy of the newly generated model and then make the generator re-generate the same model and then fix it in that one (sadly I do not know, whether I can insert gifs of cartoon characters letting out a hopeless sigh - so use your imagination for now).

# 2022-11-08
Finally things are happening. We have <i>The Answer</i> to life, the universe and everything. The answer is 42! Well, okay Douglas Adams did actually give us that answer in his book, but his books also stated that the Earth was made as a large computer to find the question for which the answer is 42. The question is discovered here! It is: "What is the amount of classes in ISO standard added to the major ISO namespace groupings?".

Well - let's evaluate: What is "the amount of classes in ISO standard" (39 as far as I could count) added to (that's a "+" ) "the major ISO namespace groupings" (there is Iso20022.Message, Iso20022.Business, Iso20022.Properties - so that's 3)?  => What is 39 + 3? 42! *BOOOM - The universe implodes and everything start all over*

Anyway, we have cleaned up the connectors a bit. It is still one big pile of random thrown-in classes in a BIIIG white canvas, which was subjected to modern Art of Red/Green/Blue squares instead of circles. The best part now is that we can actually try to an*need* a business process. Who knows - it might also be a glitch in the generator. Time will show, but let's us analyze the rest in order to find more problems to fix.