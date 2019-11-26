require 'albacore'

build :build do |b|
	b.sln = "DiplomaSolution.sln"
	b.target = ['Clean', 'Rebuild']
	b.prop 'Configuration', 'Release'
	b.logging = 'detailed'
end

desc "Publishes web application"
build :publish do |m|
  m.properties={:configuration=>:Release}
  m.targets [:ResolveReferences, :_CopyWebApplication]
  m.properties={
   :webprojectoutputdir=>"/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution",
   :outdir=>"/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/bin"
  }
  m.solution="/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/DiplomaSolution.csproj"
  m.command="/Library/Frameworks/Mono.framework/Versions/6.4.0/bin/msbuild"
end
