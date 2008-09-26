task :specs do
  sh "Libraries/Machine/Specifications/Machine.Specifications.ConsoleRunner.exe Source/Machine.UoW.Specs/bin/Debug/Machine.UoW.Specs.dll --html Specifications.html"
end

