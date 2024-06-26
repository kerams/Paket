﻿module Paket.InstallModel.Xml.SystemSpatialSpecs

open Paket
open NUnit.Framework
open FsUnit
open Paket.TestHelpers
open Paket.Domain
open Paket.Requirements

let expected = """
<Choose xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <When Condition="$(TargetFrameworkIdentifier) == '.NETFramework' And ($(TargetFrameworkVersion) == 'v4.0' Or $(TargetFrameworkVersion) == 'v4.0.3' Or $(TargetFrameworkVersion) == 'v4.5' Or $(TargetFrameworkVersion) == 'v4.5.1' Or $(TargetFrameworkVersion) == 'v4.5.2' Or $(TargetFrameworkVersion) == 'v4.5.3' Or $(TargetFrameworkVersion) == 'v4.6' Or $(TargetFrameworkVersion) == 'v4.6.1' Or $(TargetFrameworkVersion) == 'v4.6.2' Or $(TargetFrameworkVersion) == 'v4.6.3' Or $(TargetFrameworkVersion) == 'v4.7' Or $(TargetFrameworkVersion) == 'v4.7.1' Or $(TargetFrameworkVersion) == 'v4.7.2' Or $(TargetFrameworkVersion) == 'v4.8' Or $(TargetFrameworkVersion) == 'v4.8.1')">
    <ItemGroup>
      <Reference Include="System.Spatial">
        <HintPath>..\..\..\System.Spatial\lib\net40\System.Spatial.dll</HintPath>
        <Private>True</Private>
        <Paket>True</Paket>
      </Reference>
    </ItemGroup>
  </When>
  <When Condition="$(TargetFrameworkIdentifier) == 'Silverlight' And ($(TargetFrameworkVersion) == 'v4.0' Or $(TargetFrameworkVersion) == 'v5.0')">
    <ItemGroup>
      <Reference Include="System.Spatial">
        <HintPath>..\..\..\System.Spatial\lib\sl4\System.Spatial.dll</HintPath>
        <Private>True</Private>
        <Paket>True</Paket>
      </Reference>
    </ItemGroup>
  </When>
</Choose>"""

[<Test>]
let ``should generate Xml for System.Spatial``() = 
    ensureDir()
    let model =
        InstallModel.CreateFromLibs(PackageName "System.Spatial", SemVer.Parse "5.6.3", InstallModelKind.Package, FrameworkRestriction.NoRestriction,
            [ @"..\System.Spatial\lib\net40\System.Spatial.dll"
              @"..\System.Spatial\lib\net40\de\System.Spatial.resources.dll"
              @"..\System.Spatial\lib\net40\es\System.Spatial.resources.dll"
              @"..\System.Spatial\lib\net40\zh-Hans\System.Spatial.resources.dll" 

              @"..\System.Spatial\lib\sl4\System.Spatial.dll" 
              @"..\System.Spatial\lib\sl4\de\System.Spatial.resources.dll"
              @"..\System.Spatial\lib\sl4\es\System.Spatial.resources.dll"
              @"..\System.Spatial\lib\sl4\zh-Hans\System.Spatial.resources.dll" 
            ] |> Paket.InstallModel.ProcessingSpecs.fromLegacyList @"..\System.Spatial\",[],[],Nuspec.All)
    
    let ctx = ProjectFile.TryLoad("./ProjectFile/TestData/Empty.fsprojtest").Value.GenerateXml(model, System.Collections.Generic.HashSet<_>(),Map.empty,None,Some true,None,true,KnownTargetProfiles.AllProfiles,None)
    let currentXML = ctx.ChooseNodes.Head.OuterXml |> normalizeXml
    currentXML
    |> shouldEqual (normalizeXml expected)