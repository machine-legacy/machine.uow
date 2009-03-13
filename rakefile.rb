task :specs do
  sh "Libraries/Machine/Specifications/Machine.Specifications.ConsoleRunner.exe Build/Debug/Machine.UoW.NHibernate.Specs.dll Build/Debug/Machine.UoW.Specs.dll"
end

