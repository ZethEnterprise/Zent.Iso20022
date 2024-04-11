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

# 2022-11-21
Okay. Let's take the file (ERepository.iso20022 Model (Auto-Generated).svg) and analyze it. We can call the file for "Second iteration" as we now are going to remove, what we do not want. Let's see... Ah! BusinessProcess! We can remove that, because - well, let's be honest - what use is a process in a xml file. Well - joke aside. But what is it for? Okay. Looking at the iso20022-file and reading up on it, then I can actually see that it doesn't mean anything in the XSD creation sense. BusinessProcess contains a list of BusinessRoles, which basically just describes how ISO20022 looks at the different roles in general. One could see it as a Terminology over the different kind of roles like: "OrderGiver", "Trustee", "BuyersBank", etc. BusinessProcess or BusinessRoles are not used anywhere else and therefore it can be removed.
Let's remove some of the "simple types" such as Date or DateTime. Constraints are just a descriptive part of the simple types, but it becomes interesting, when talking about MessageDefinitions, so let's keep it for now.

Now we have trimmed it a bit, by removing a few in Iso20022.Properties namespace. Let's see, what the other stuff in SimpleTypes are.

* Doclet: Doclet seems to be just a neat descriptive component, what I got from it so far is that Doclet (as a program) generates the documentation of Java source code, just like Doxygen (D'oh! I've should have seen this one coming a mile away). Funny part it that it got me thinking: "That the heck <i>IS</i> a ECORE file anyway?". I know for sure that it is not a file in Evil Corp (if you don't know that reference, then watch a very nerdy TV serie). Searching a bit on, what ECORE actually is, I get a neat message that it is related to Eclipse (Yes - Java. The dots are connected). One funny thought: "What happens if I try to open the ECORE file with Eclipse?". That is a thought for another day, though.

* iso20022:Quantity, Amount, Time, Rate, YearMonth, Year, Binary, Month: These are so to speak just simple types describing quite nicely what they are meant for.

* (simpleTypes) example: Well, that is very obvious what it is about.

# 2023-05-03
We are back! Sorry about the delay - had a lot to attend to, but now we are back once more! So, let's see where we left off. It seems to be a long time ago since we had a check-up in there. Ah! The second iteration file. Should we see, what else we can remove from that iteration, which does not make any sense in the XSD creation sense. Let's look into <i>XORS</i>. XORS actually describes <i>either one must be present, but only one</i>-rule on the component it is linked to. This rule seems more businessey-like than anything else. So perhaps a bit more conceptual like "that either this field field is present in this subchild from this parent or in this sub-subchild from the same parent".

# 2023-05-10
The <i>XORS</i> got <i>impactedElements</i> and <i>impactedMessageBuildingBlocks</i>, where either one is filled out with an ID (and God forbids both to be filled at the same time). So, in the generation (if we really want a generation of this) of the validation rules in a higher level, means that we need to look closer at this part. In a pure Object/class generation it does not matter at all. 

So - what's next? <i>Code</i>! What in the (blasfemis) World is <i>Code</i> doing here? It is a part of <i>Iso20022:CodeSet</i>, which contains <i>Constraints</i>, <i>SemanticMarkups</i> and <i>Examples</i>. So Code got <i>CodeName</i> (47!).
I actually found a good example. CodeName "DIST". I got two almost similar TopLevelDictionaryEntries for the same thing. The one of type CodeSet has this Code with the CodeName "DIST" and that Code's name is "Distribution". But here's the thing: That TopLevelDictionaryEntry got a derivation class, which is also a TopLevelDictionaryEntry of type CodeSet. This one links back to the previous one through the field Trace and it got an <i>Example</i> which ontains "DIST" and it has a Code with the name "Distribution". This means it actually have two versions of the same and here's the thing: The one with the without the CodeName is <i>Provisionally Registered</i>. So it seems like the ones <i>Provisionally Registered</i> are used as "Submitted for evaluation and not usable in real life".