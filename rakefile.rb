require 'albacore'

build :build do |b|
	b.sln = "DiplomaSolution.sln"
	b.target = ['Clean', 'Build']
	b.prop 'Configuration', 'Debug'
	b.logging = 'detailed'
end

