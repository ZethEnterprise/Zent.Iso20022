This is the story of how this fun sparetime project went and what I've learned while playing with it.

# 2022-10-09
There has been some writings on the model. Finally I figured: "It's time to submit what I have". After working in the LinQ department it seems a bit unimaginable to figure out this model of Iso20022 eRepository, which is why we are trying to draw it. I can say it is not an easy task, because of the sheer size of that file and eventhough I am proud owner of LinQ scripts, which can generate a dumpable and collapsable model.
I finally managed to take some of the easy classes and draw them up, but when I moved over to the semi complex ones, it becomes even harder. That is due to the inheritance, which is invisible in the Xml model. I like abstractions just as much as the next, but looking through the data it is impossible to make sure that you get every needles in the haystack. Which is why I have decided to abandon this UML model until I can make a neat and queriable model in LinQ. That will help me locate all of the sweetspots, where I am missing some inheritance. That is also the key to find the correct way of making the abstractions in the generated model in the future. One further note - couldn't it be nice to actually be able to generate a PlantUml model alongside the actual classes?

It was fine to use the PocUnderstandingIso.linq file in order to generate the PlantUml diagram. The problem was however that we cannot get the model right. Some of the dynamic created objects were actually having some "<fieldName>Missing", which meant that the linking of the objects did not find any object with that ID. That sounded like a problem. After some investigation into this part, it actually showed itself off as a false-positive. Meaning, not all attributes with this name referenced to an object. One of these attributes were "Type". In many cases it referenced to an object, which it was a type of, but other cases as in "SemanticMarkup" is had a human understandable meaning instead.
While writing up the PlantUml drawing another issue was discovered. The idea was to make connections between which classes referenced to oneanother, but that again proved to be an impossible task. A lot of places it seems straight forward, but then again some other classes could reference to multiple kinds of classes, which means some sort of inheritance was needed in places like these.
Therefore a new file was created called "PocUnderstandingIsoInBabysteps.linq". The meaning for this was that we take it slow; build on the raw model as we go. First we started out with just a list of XElements. That wasn't so hard and it actually showed that the numbers of children/xmlTags in the ISO20022's eRepository sort of matched up. That meant that we had all the objects in one list. That was the goal as we would want to use that as a lookup dictionary for the linking process.
On top of that the ID was extracted; that's cool - one step closer to that lookup dictionary. The next problem was then: "What about the ones without an ID?". That was a good point and it had to be taken into account. So to begin with these ID-less objects are just subchildren to a child with an ID. The next problem, which we would face is the "type": which type and where do I get it? 
There are two different kinds of types: "XSI:Type" & "XmlType" (or "XmlTag" one might say). So we might want to be able to distinquish between those two kind of types in the future, which is why the raw Model is extended with there fields or metadata.

# 2022-10-16
So how to detect the inheritance within the object? Let's find out in this next round of testing. Perhaps the first part is make the raw model whole. By this the meaning is of course getting the properties, which is not a direct descendant of the object. Yeah, after a quick review of the properties to dictionary code from previous code, it is obvious that the same code cannot be applied there: Some parsing and interpretation is needed. 
So - there was a small inconsistency in the XElements.Descendants - just remember that the Descendants takes ALL Descendants and not just the direct descendant. You are going to kick yourself for forgetting that part.
We didn't manage to get that far, but for now we have a dictionary with all of the raw models and their IDs. This will come in handy, when the method "CorrectXmlProperties" are getting implemented. This method shall translate all of the properties into objects based on the newly created dicionary and it should also isolate properties such as "Type", which can either be a simple type or a "RawModel" as found out on the 9'th of October.

The most important part is to remember: One step closer to a descriptive model is a step closer to understanding the ISO20022 completely.

# 2022-10-21
So finally there is generated a PlantUml file from LinQPad query. The funny part is that it is not previewable. To think of all the different possibilities that a specific type can be in: Missing a "MinOccur" field or a different kind of "DerivedComponent" fields or something like that. Even if we look at the list as distinctive, it is still too many components to make it PlantUml previewable. To top that one, the puml file generated is still too large to be easily trimmed down by hand (I should know).

# 2022-10-25
It finally happened! Something, which looks like a PlantUml model was generated. It is now linking the RawChildren and the XmlProperties to each other. This will minimize the manual work needed afterwards. This means that the next step of understanding this weird model is in the PlantUml project - now we have the base code. Next step is to modify the base code to model the structure in some neat way.

Boy, what a journey! Sadly we are not there yet. Looking at the preview in VS Code (yeah - using VS Code with PlantUml plugin for this modelling) the image is going to be very wide. I neat way of discribing it is to take all the lines in the ERepository file and merge them into 50 lines with equal lengths. Nah - just kidding, Notepad will crash before that happens due to overflow of characters in one line. Jokes aside, the image is going to be very large (in a wide scale) and exporting it to PNG cropped it up, which meant that it only took about half of the image (or perhaps two fifths of the image). Trying to export it as PDF or HTML failed as well. The SVG file was the only one seemingly being capable of containing all the data in one file. Please note that the "Page 4x1"/"Page 2x2" has been added in hopes of PlantUml getting a fix out on that part, because it is not working as it is only exporting one image. By some small search it seems like it is a bug, which hasn't been rectified just yet.

# 2022-10-26
YAY! The model has been re-exported as SVG. It is still a mess, but I finally got it cleaned up a bit. It is still a mess, but now it is organized chaos instead of chaotic chaos. Adding some namespaces (which by the way is bad to do in a 600+ line PlantUml file - you might skip something and the namespacing vs. packages are not the same) with some colors and pseudo-organizing it gives a better overview. What's left now is actually checking the model through for errors to see what is missed. One thing though. The Model should NOT be auto generated any longer (don't want to loose my hard work (imagine a laughing smileyface here) ).

One thing I can see (by looking into **iso20022_MessageAssociationEnd**) is that it is not complete. Those annoying traces and types are still an issue. So there is two things to do going forward here: Fix the generator or manually fix the model by walking through the forrest of Types, which <i>could be</i> an ID or a string (sigh).

I think the plan for now is to make a copy of the newly generated model and then make the generator re-generate the same model and then fix it in that one (sadly I do not know, whether I can insert gifs of cartoon characters letting out a hopeless sigh - so use your imagination for now).

# 2022-10-27
Funny story - Do you know this YouTuber "Let's game it out"? He is playing games for a livin' and actually tries out the limits of the games that he plays. Limits such as how much can you misuse the game and beat it up in a funny way. Limits like "how much stuff can you spawn in the game before it actually lowers your framerate to basically nothing. The reason I am asking is: We are sort of doing the same with LinQPad right now. The last couple of days I managed to get weird exceptions in <b>PocUnderstandingIsoPlantUml.linq</b> - running it too many times in a row can break the Results window. I will also give you the resolution for getting it to work again without restarting everything. Go to Menu => Query => Cancel All Threads and Reset. From my investigations (didn't go into details though) is thAt it is using Edge for displaying the results and there is some memory leak in that area.
And furthermore! I actually got the model to be built to more or less look just alike the previous (version 1) one.

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
# 2024-02-28
Today I finally got something. I have been working on the PocIsoRepoParser, because I want to move forward on actually making the model. This PoC is actually a PoC trying to make a model based on the elements from the Pain. I figured that I was sourcing the name wrongfully when parsing the SimpleTyped elements - so I have added a bit in that regards. Something still bugged me and that was actually this part of the CodeSets. I know they are Enums to be frank - but the ID of the element did not match with the values they should contain. Then it dawned on me! That is what the Trace reference is used for. It shows the actual values, which can be in this field! It feels good, once you figure out these small things. But that also means that we need to run through the whole file in order to find all "xsi:type=iso20022:CodeSet" and transform them into Enums - but only the ones mentioned in your traces.

# 2024-03-07
So... We meet again Mr. Bond... Treasury Bonds... (Ba-dum tish) Sorry - one bad joke for the day, but anywho - back to business. I found the missing link between the CodeSets and the objects. It just turned out to be even more complex than initially thought. The interesting part is that there seems to be three different kinds of CodeSets:

1. Simple => meaning every information I need is within my .iso20022 file
2. Simple+ => meaning that we have some informations within my .iso20022 file for what we can call the enum, BUT (there is always a 'but') the actual XML code, which should be translated into these enums are actually something coming from the 'ExternalCodeSets_1Q2022.xsd' file
3. NotSimple+++ => meaning textbased field restricted with Regex to describe some arbitrary lettered/numbered code, which can change in an instant (like currency codes)

I haven't found all of them just yet as LinqPad at this moment is just looking up all of them (meaning looking them up again if the occur more than once). That will be the next step. Them only one of each out.

# 2024-03-08
I have finally put in some more work on the IsoParser. I think I will pause the LinQPad part of things now and actually see how I can manually can make a parser for a XML just like the XSD.exe does it. My idea is that I want it to be able to generate two different types of models: The XSD.exe type, where it basically makes it all the way in depth - and a slim version (just like Playstation does it. My idea is that the slim version should minimize the nesting by the usage of Abstract classes and make the model nicer to look at).

# 2024-04-10
I FINALLY CRACKED IT! *"Cracked what?"*, you might say. I'll tell ya. My initial plan was to generate classes based on this ISO-model, which I've just made, and then use T4 templates for code generation. T4 templates are very handy for code generation and the normal usage of them is to execute them on the compile-time. In this case I didn't see the benefit to make it on the compile-time as the idea is to use it as a NuGet or a Tool for generating just the files, which you need. Therefore cloning this repo and generate the code from there was not the initial idea. Therefore I've looked into the runtime T4 template, which is something you can call on the fly and generate a set of files based on that. This is more to my liking as we can now utilize it as a tool like the famous xsd.exe tool or something similar if that works for you as well.

Anyway, my journey started a bit bumpy; copied my parser and internal model to a PoC solution; created a PoC runtime T4 template. This worked more or less out of the box, but now things got interesting. How do I feed this T4 template pieces of my model? That let me to a long time researching with examples and other peoples experiences. I have to feed the T4 template some parameters and I did that in multiple ways. I ran into a lot of errors, where I was missing either this DLL or that one (I will try to add the names at some point for the Google Webcrawler to pick them up and speed up your own research). There were various ways people had tried it and some also said that a normal .Net Core or above could not run with the T4 runtime template parameterized (and that is completely bonkers - because I actually got it to work). To keep it short: Import System.Codedom; create a T4 runtime Template file; build; create a U-SQL class file; make it partial to the T4 Template file and BOOM! now you have the parameterized T4 template for runtime transformation. *...sighing so ever lightly...* Yeah - I was beginning to loose any hope of using the T4s and just make it myself. But now the PoC works and I can begin to explore the model (and it is running with .Net 8!!!).

# 2024-04-13
So the first class has been sort of generated. So now there is some small details getting added to the thing. So let's see if we can make that first thingy serializable... HA! Lovely! Mr. Breaker is back in business. Just like LinQPad had issues due to the sheer size of the file '20220520_ISO20022_2013_eRepository.iso20022' (and rendering afterwards), so have I met my match in Visual Studio. The XML editor can only take in 10MB of a file, but luckily you can change the registry key and change it to something larger.

Execute this in a PowerShell window in order to make the setting for a larger size (in this case it is 100MB):

`$vsWherePath = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
$installPath = &$vsWherePath -all -latest -property installationPath
$vsregedit = Join-Path $installPath 'Common7\IDE\vsregedit.exe'
& $VsRegEdit set "$installPath" "HKLM" "XmlEditor" "MaxFileSizeSupportedByLanguageService" string 100`

 One thing that is a bit interesting is the identification of the schema namespace => How should we do that??? Uh - the generator will also take the version number of the assembly and put it in just like the XSD.exe does. Next question is to see how it can change based on builds, commits, pushes etc.?

 # 2024-04-14
 So - now all the classes schould have been generatable, though they might be a bit faulty. To summarize: The main classes are generated with some human readable namings and not the xsd tags like the xsd.exe file did. We have some of the normal attributes (xsd namespaces) on the class as well as summaries on the fields and classes (if any definition was present in the XElement entry of said object). Next step is to add the XML tags on the entries and then see how the classes then will look.

 # 2024-04-18
 Hello fellow nerdies! We have now generated the classes as much as possible (for now) and the next step is to generate in a way such as the classes can be used for serialization. There is still some quirks about the simple types of properties, which we need to get solved first. One of them is the infamous field 'IBAN2007Identifier' a.k.a. the IBAN field, which will in xsd.exe be translated into a string stored as an object as it can also be a generic typed class. So far I know at this point this 'IBAN2007Identifier' will not be made as a class with the current model. The question is whether it should - so now I have to debate with myself pros and cons on this matter.

 # 2024-04-20
 I've found some spare time to have some more fun. Uh - this is getting better as we are going along the way. So to give a quick recap of what kind of shenanigans I've been doing in my PoC. As you all might remember from last time - I've been playing around with the obsticle of IBAN2007Identifier for which I have created a new Property class, which is a child of SimpleProperty. It is called 'RegexStringbasedSimpletonObject' in order to signal that this is a stringbased field with a Regex rule-base. So far so good, but here's the thing! How do identify that the property is of this class? The problem here is that the T4 runtime template is actually compiled into an assembly for itself and only knows of its own little world - meaning that everything I do within this template and its partial class will not extend further out than that. This means that I cannot use something as simple as "thisObject is RegexStringbasedSimpletonObject".

 But fear not! I think I've located the answer in my search on StackOverflow (my kudos goes to a Phillip Scott Givens) with this link: https://stackoverflow.com/questions/22182441/calling-a-class-in-the-same-project-from-t4-template

I think that this calls for a newly structured PoC, where I put a bit more effort into making it clean to begin with. So... It is finally managed and this thing did work like it should. I had other sources telling me to adding the assembly from the $TargetPath, but just having it in two separate projects and letting one reference to another works out of the box. This way the compiler will build first the dependency and use that reference in the next project without any issues. It runs so beautifully well. So yet another thank you to Sir Phillip. These T4 templates got a lot of various hits when trying to look up any help on a VERY specific and niche functionality, which people usually don't explore. But not me - just call me Dora (the Explorer) as I am not the one to back down from a fight. I want my code to do as I want and not something else.

# 2024-04-24
Yesterday on another train ride home I thought of something... Why do I make these weird class names for the simple types. We do already have a representation of them in the ISO model! So why not use them. First of - RegexStringbasedSimpletonObject...???!!! That it represented was actually an IdentifierSet. Looking back at the model the iso20022:Text can also contain a Regex. But would we always translate an IdentifierSet into a string or could it be that we wanted to translate it into an object, which can actually have some properties? See! This is what I did wrong from the beginning! iso20022:IdentifierSet and iso20022:Text can have more or less the same properties (pattern, minLength, etc.), but they might as well mean differently in our generator!

(At this point in time all I could think of was a funny quote from Stifler in one of his Bloopers: "Being dumb is so smart", but fact checking it got me some other quotes - whether they are said by those persons or not I have not checked -

1. "Everybody is a genis. But if you judge a fish by its ability to climb a tree, it will live its whole life believing that it is stupid." - Albert Einstein
2. "A fool thinks himself to be wise, but a wise man thinks himself to be a fool." - William Shakespeare

I just thought that those quotes could work as a nice motivational quote of the day. Have fun with them.)

# 2024-04-25
I GOT SOMETHING GENERATED, WHICH CAN BE DESERIALIZED INTO AN OBJECT AGAIN!

Okay... All fairness... We are not there... Yet... But we are now at a new break point in the development of this nice and cool tool!!!!!

I AM SO HAPPY!!! <insert GIF of dancing dog> - Another thought... How can I insert a picture or GIF in a README file for Repo reading? Must be tested out...

![The classic cat!](https://upload.wikimedia.org/wikipedia/commons/0/04/So_happy_smiling_cat.jpg)

# 2024-04-26
(I found another cat I had to add. It will come in the end of this update.)

Hi there everyone! So we finally made a runnable version of our code. The next part is to trim out the faults such as amounts are not strings etc. Amounts is a bit interesting - I can see that 'InstructedAmount' is translated into an object of 'ActiveOrHistoricCurrencyAndAmount' when generated through the xsd.exe tool, which is quite interesting. Looking further into this field I can see that it 'isComposite'. What does that mean? I know what the word means, but what does it mean in this regime? As far as I can see is that it is not as 'Simple Type' as first thought of. So it must therefore mean that it is a complex simpleton instead.

![Hi There!](https://c.tenor.com/5Wox2TQlBowAAAAC/tenor.gif)

# 2024-05-02
So. I'm sitting here. Thinking of the Identifier component. Perhaps I want that one to be a class for itself, but should all the Iso20022 properties be classes for themselves? I think not - that would make a serialized representation of an object be very object heavy with a lot of levels to go through. That would be waste of good memory. So how to determine what to classify and not to classify?

# 2024-05-16
Hello fellow ISO nerds! Sorry for the radio silence... The last couple of days I've been using the spare minutes trying to generate Enums. Fun fact is tat not all CodeSets have an external codesets. What's even more curious is that  not all codeset contains code elements!?!! And here's why: Some codes are too dynamic/volatile for them to lock them in the ISO standard, others are described in other ISO standards. Some of these are e.g. CountryCode - it is covered with ISO 3166. Codes like LanguageCode are the ones marked as "validation by table", so it is codes that this standard does not want to control.

# 2024-05-20
Hi there (once more). Yeah - the writing/coding and the committing doesn't always happen on the same date and time. Anyway. Let's look into this codeset thingy. The codeset thingy without any external codesets, trace codesets or privately owned code sets are actually quite interesting. One of them was this LanguageCode, which doesn't contain anything but this "constraint" of "ValidationByTable". Tracing it back to its origin in a schema I can see that it is at least used in the acmt.001.001.02 (Investment funds). I located that schema and used xsd.exe on it to see, what we should expect. The answer is: String.... Oh. But then it must be some RegEx based string??? No. So... Just a "Normal" string? The answer is oddly yes. Then perhaps there is some XSD validations on it being in some specific way? No. Hence - just a string. Shoot! That's not very straight forward way to go about this. I've created a small LinQPad file for quick parsing the CodeSets into categories:
1. Externals (252 codes)
2. Traced (2458 codes)
3. Standalones (1201)
4. Volatiles (39 codes)

Okay... It took 1.335 seconds to run without rendering. That might be acceptable. There were also something fishy about the traced codes. The thing is that it was Enums inherting from other Enums, which is a big no-no. So how do they do it? Furthermore: Why do they do it? From what I could tell the "Master Enum" contains all the codes, whereas the "Child Enum" contains only a subpart of the values. So in order to not write the same Enum Value and its code twenty times, you have the codes in one place and tells the consumer that this is the only ones that you care about. So messy. The idea is great, but if you don't limit it even further then... What's the point? You could easily say: CodeSet(trace=MasterCodeSet)/Code(trace=MasterCodeId) and then the CodeSet(id=MasterCodeSet,Description,Tags,Etc.)/Code(id=MasterCodeId,Description,CodeName,Name,Etc.). This way you would really save up some space. Even make the CodeSets even more intuitive would help a lot by giving them some sub-codeset names or a property that says: Standalone, External, stringbased, Regexbased, Interited. WOW! If someone from ISO20022 project reads this and things: "Hey! This guy got a point! Let's remake this!" - How awesome would that just have been?!?! Probability? Aaaaaaaaaaabout 0.00000001‰.

So... I have finally made the CodeSet lists in those four categories in LinQPad and got them parsed into something that I can use. Next step: Generate the code for them, but we don't want all of 3950 Codesets written in. Uh! Sorry, 3911 CodeSets (the last 39 are the Volatiles ones, which cannot be translated into an Enum when push comes to shove). Next stop! More code generation!!!

![Woo Hoo!](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQVsp6iETQvP5P3fSl4HwyjGkWL2w1A3mm6deJ3e5PcwQ&s)

# 2024-05-23
![D'oh!](https://i.pinimg.com/474x/17/01/77/170177e2f0a024564a2f78db42599e23.jpg)

Soh... I got the Enums merged into the same Dictionary.. Or so I thought. It turned out that some traced CodeSets are also to be located in external code sets... But what?! Does that make any sense? So I looked into the CAMT.006.001.09 and surely enough. It does not work as an Enum. It works as a string-based thingy - so... Is that to be expected? Now to think of what to do in that relation.

# 2024-05-24
What to say...? <i>"Education never ends, Watson. It is a series of lessons, with the greatest for the last."</i>
Aaaaaanywho. Let's get moving: Some small checkpoints: all of the External CodeSets will only be translated into strings - WITH limitations of some sorts. I will therefore do some magic to add it as an Enum, but also as the Raw input as a string. In that way we should be able to get the best of the two worlds. Well - best of one world and the lesser evil from another.

# 2024-07-22
Hello fellow ISO20022 hunters. It's been some time since last (textual) update. Todays topic is (still) CodeSets - a.k.a. Enums... Or so I originally thought. I've been looking a bit into (and fact-checking) some of the various codes in order to see, if I have correctly figured out how to translate them. I came across this one 'ExternalDebtorAgentInstruction1Code', which is really annoying. The reasons:

a. Only the inherited code set does not have any codes within it
b. There is a link to the External Codes file
c. The External Codes file... YAY! The external file from 1Q2022 did not have any codes for it, but 1Q2024 has - updated a month ago (does this mean that there is a new eRepository as well - jeez...)
d. Well - that wraps it up - if there is a code on either parent or child in the inheritage tree, it should/could be considered an enum

So... That was interesting.

![Annoyingly thinks for myself](https://media1.tenor.com/m/TO2kHrt7xIIAAAAC/work-thinking.gif)

I think that might concludes my train of thoughts on the Enum problem. So this is the next step:

1. Locate all Coded CodeSets and their parents
2. Also locate all nonCoded CodeSets with their Coded parents
3. Link all Coded CodeSets with their External opposites
4. Generate Enums

# 2024-07-25
Hello fellow nerds (in the best meaningful way)! So... Looking into these new codes from the new ISO repository and External CodeSet files I think I am about that place in time, where I can make an educated guess: Everything from External CodeSet file is seen in the XSD files as stringbased restrictions. This means that my set will actually be able to enrich this part even more. It also means that I might need to add some possibility of chosing between having a "RawStringOfField" and "EnumOfFieldWithRealFieldName" or the other way around (in order to not making breaking changes when people are migrating from their versions over to mine).

(oddly enough I am talking about others using mine - who knows whether that will be something that will be happening at all...? Oh well - one can hope)

But this also means that I can now safely Identify, which codesets should be used as enums and which should be hybrid or totally ignored. So now I can actually make a class for it and store what is needed (it was that TSIN.005 that did the trick - oddly enough it was found on this link: https://www.iso20022.org/iso-20022-message-definitions?search=tsin.005.001.01 but not on the one I previously used: https://www.iso20022.org/catalogue-messages/iso-20022-messages-archive?search=TSIN.005.001.01 - so... Why is that? Shouldn't the Archive have everything??? Or.. Perhaps the Archive only contain the old messages that are deemed decommissioned).

# 2024-07-31
Hello fellow code hunters... Yeah. The last couple of days I got a bit annoyed and focused a lot on getting the enums done. I think we got there. One thing though (okay.. Two things):

1. I was so happy when the new standard and external codeset was published, BUT (because there is a but) one of the codesets (here is the sinner *HMPF*) 'ExternalUndertakingChargeType1Code' is set to be externally referenced, yet it is not to be found in the latest update from the Second of June 2024. It broke my world. And that led to the next thing... I wanted to help them and report my findings... You cannot say that this CodeSet is externally referenced and even link to where it can be downloaded and not even have it in said file.
2. ISO20022 webpage got someway for people to get into contact with them, buuuUUUUuuut (because there is always a but) you have to be in a company of some sort to do so... My project here is a hobby project, because coding is fun. How come single individuals cannot help finding bugs? What if one is a freelancer and wants to work on a project within this area? What if one is working in a company that are working in a completely different sector than theirs? Would they even be allowed through the first screening? Sometimes "Open Source" is also good - yes, yes I know that this is not code as it is a standard described in XML and if you are lucky and you can get it to work right away, one can use Eclipse to visualize it(/use it?). But Open Source as use the community to help one out detecting some stuff or like those Hacking Reward systems, where everyone can help out by reporting openings no one ever thought of. So... Yeah - that was my update. And by the way... Why does the Archive showing so neatly how the various versions of files are linked to eachother, while the iso20022 files does not? It is only Latest MessageSet or Archive MessageSet.

# 2024-08-08
Hello fellow coders! So... Fun fact: "pain.001.001.02 has its message definition identifier as xml tag. So does pain.001.001.01, but this version is not injected into the eRepository."

So... How 'bout that? Isn't that weird? This just means there is another weird edge case to take care of.

# 2024-09-01
Hello fellow Night Owls! A small update. I've been having fun with my code generator. Lost sight of it all for a bit. Well... I wanted to make it nice and complete at the same time, while I needed to make changes in my model AND in my templates. So I tried to stay focus on multiple points at the same time. You all know what that's like, right? You lose yourself in the details and gets nothing done.

That is why I've introduced my first test class in this project!!! (Also because it was always my intension to try out Sonarcloud as a free version for this thing - but I had to have something worthy of Almighty Sonar!)

Anywho - that's not important right now. What is important is that I've actually shifted my focus! I focused on what I actually wanted as an end result. Running everything (though it is quite fast with the threading'n'all) I don't want to run it every time to cherrypick a few cases/classes for proof of my work is correct. Therefore I have now created a test case of each template I have, so I can actually simulate what I have done and make small changes and see them right away. Furthermore it actually provided me with the contract between the class generator and the model generator. So I will more easily be able to make my model so it adheres to the interface specified/requested by the consumer. I'm always preaching at work about the importance of making clear cuts in order to distribute workload across multiple developers so everyone can help at any given point in time. That's a story for another time - just like the time I had a colleague telling me that some system I was taking over was like Command & Conquer - "You have a black map and can only see this. When you slowly work with the system you'll explore more and more very slowly". I personally like that reference and it actually tought me to address my systems in a specific way:

1. Always be curious and explore
2. Don't limit yourself to your own system (explore the allied's or enemy's territory)
3. Explore the business domain, but also the other connected business domains

Those three golden rules will actually give you insight in your client's world and provide the best help - whether it is merely being able to tell them "Oh, that thing there, that's in this system, this is your vendor maintaining it. Here let me give you my contact person for them.". The more you know about your client/own company the more valuable you'll get in terms of service you can provide and guidance you can give as you know how everything interconnects. Now - there are probably someone out there, who will say: "Well, that is a generalistic or an architect profile. I want to be a specialist within a small area." - to that I'll say. "So? Why is that erasing you path towards a specialist? A specialist only focuses on a small area, but knowing what is talking to your small target area will also help you know how to focus/direct your skills to a problem for the best solution - not just 'good' solution."

So. Just to round it up. I think the only template we are missing at this point is the fuzzy Enums - the ones that are sort of enums-BUT-NOTs. The ones where we will have external lists and yet internal lists of enums, but the XSDs are always describing them as Strings with RegEx Patterns etc. That is for another time.

# 2024-09-19
Hello fellow Nerds (meant in the most positive way ever possible as being a 'Nerd' is - in my mind - very positive). It's been a while since I last wrote in here. I was so pre-occupied with coding that my tunnel vision was soo narrow it was like spotting the end of the long tunnels of the Beor Mountains (if anyone got that reference - if not it would be spotting the exit from the center of Mirkwood). Anyways - It became troublesome working in a waterfall way or event vertically implementing bits and pieces, so I began to make unit tests (QAs spotted clapping in the distance). I began to make an interface-based unit test for the class generation and then move that over to the model generation to generate the same kind of model object matching that interface. I think I got away with it very nicely. I'm not finished just yet. Far from it. But I did manage to generate a model for the Root element and the initial class element, which is based on that root. That is very good, because that means the first part is over. I managed to generate some abstract classes and their derived classes and now to actually generate the model obejcts for it.

This is here, where I found out something interesting. An Abstract class could have a derived element in a form of a class, but also as a text but even enums!!! So. The current working plan is to make some wrapper inner classes in the abstract class for those kind of elements - but that also means that we have to make changes for the class generation.

# 2024-11-15
Hello fellow Nerdies. Jeeeeeez! Long time since last commit (and update in the READMEs). Sorry about that. So... A small update: I've been working on the interface agreement for upholding that what comes in produces what should come out. So we are now looking into the nice world of TDD (the famous Test Driven Development - or at least something that should resemble that). Currently I am looking into the agreements for the Polyformiphic scenarios. So right now I am generating some packages, which would contain the whole package needed to be produced. I had a couple of concerns that I did not have everything in my inherited classes in order to generate the full XML part of it. Now I am not so sure anymore (the con of mostly doing this project in transit in the train and not being able to get a seat every time => you forget where you are from time to time and which brilliant idea you had last time). That is also the reason why I am trying to document more and more the code - it is to benefit you guys'n'gals, but also to remind me what the ... I was thinking at that time.