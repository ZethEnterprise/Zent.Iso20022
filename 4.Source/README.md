This folder contains the actual Visual Studio project for creating the ISO20022 Data Model to include in your project.

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