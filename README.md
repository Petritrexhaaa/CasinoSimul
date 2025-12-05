Hello professor, or anyone viewing. This will serve as instructions on how to run the project, and how to setup the database.
First, clone the repository, or download it as a .zip, your choice. Open it with Visual Studio (the purple one, not the blue one.)
Once you've opened the folder, double click on the .sln file. This will make a solution explorer appear. 

Next, you need to also download the database, to make register/login work. I'll tell you how to do it:
1. In visual studio, go to tools -> NuGet Package Manager -> Package manager Console.
2. On the terminal that just opened, copy paste this: dotnet tool install --global dotnet-ef
3. Hit enter, and it should download the tool. Once it finishes, copy paste this: Update-Database
4. And that's all. Next, feel free to start the project.

Now you can actually run the project. To do that, make sure you've clicked the .sln file, and click the green arrow at the top to build the project.
Wait a little, and the project should start.
