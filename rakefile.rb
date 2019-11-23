require 'albacore'
require 'rubygems'

msbuild :msbuild do |msb|
	msb.solution = "DiplomaSolution.sln"
	msb.targets :clean, :build
	msb.properties :configuration => :release
	msb.command = "/Library/Frameworks/Mono.framework/Versions/6.4.0/bin/msbuild"
end

desc "Publishes web application"
msbuild :publish do |m|
  m.properties={:configuration=>:Release}
  m.targets [:ResolveReferences, :_CopyWebApplication]
  m.properties={
   :webprojectoutputdir=>"/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution",
   :outdir=>"/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/bin"
  }
  m.solution="/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/DiplomaSolution.csproj"
  m.command="/Library/Frameworks/Mono.framework/Versions/6.4.0/bin/msbuild"
end
