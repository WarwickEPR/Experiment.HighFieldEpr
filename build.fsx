// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/build/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open Fake.UserInputHelper
open System
open System.IO

let project = "HighFieldEpr"
let summary = "TODO"
let description = "TODO"
let authors = [ "University of Warwick EPR and Diamond group" ]
let licenseHeader = "Copyright (c) University of Warwick. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information."
let tags = "fsharp science"
let solutionFile  = "HighFieldEpr.sln"
let gitOwner = "WarwickDiamondEPR"
let gitHome = "https://bitbucket.org/" + gitOwner
let gitName = "HighFieldEpr"

// Helper active pattern for project types
let (|Fsproj|Csproj|Vbproj|Shproj|) (projFileName:string) =
    match projFileName with
    | f when f.EndsWith("fsproj") -> Fsproj
    | f when f.EndsWith("csproj") -> Csproj
    | f when f.EndsWith("vbproj") -> Vbproj
    | f when f.EndsWith("shproj") -> Shproj
    | _                           -> failwith (sprintf "Project file %s not supported. Unknown project type." projFileName)

// Generate assembly info files with the right version & up-to-date information
Target "AssemblyInfo" (fun _ ->
    let getAssemblyInfoAttributes projectName =
        [ Attribute.Title (projectName)
          Attribute.Product project
          Attribute.Description summary ]

    let getProjectDetails projectPath =
        let projectName = System.IO.Path.GetFileNameWithoutExtension(projectPath)
        ( projectPath,
          projectName,
          System.IO.Path.GetDirectoryName(projectPath),
          (getAssemblyInfoAttributes projectName)
        )

    !! "src/**/*.??proj"
    |> Seq.map getProjectDetails
    |> Seq.iter (fun (projFileName, projectName, folderName, attributes) ->
        match projFileName with
        | Fsproj -> CreateFSharpAssemblyInfo (folderName </> "AssemblyInfo.fs") attributes
        | Csproj -> CreateCSharpAssemblyInfo ((folderName </> "Properties") </> "AssemblyInfo.cs") attributes
        | Vbproj -> CreateVisualBasicAssemblyInfo ((folderName </> "My Project") </> "AssemblyInfo.vb") attributes
        | Shproj -> ()
        )
)

// Adds a license header to all files
Target "ApplyLicenseHeader" (fun _ ->
    let applyHeader comment formatComment (filename : string) =
        let toRewrite =
            using (new StreamReader (filename)) (fun file ->
                let content = file.ReadToEnd()
                let header = formatComment comment
                if content.StartsWith header then None else Some (header + "\n\n" + content))

        match toRewrite with
        | Some newContent -> using (new StreamWriter (filename)) (fun file -> file.Write newContent)
        | None            -> ()
    !! "src/**/*.fs"
    ++ "src/**/*.fsx"
    ++ "src/**/*.cs"
    |> Seq.iter (applyHeader licenseHeader (sprintf "// %s"))

    !! "src/**/*.xaml"
    |> Seq.iter (applyHeader licenseHeader (sprintf "<!-- %s -->"))
)

// Copies binaries from default VS location to expected bin folder
// But keeps a subdirectory structure for each project in the
// src folder to support multiple project outputs
Target "CopyBinaries" (fun _ ->
    !! "src/**/*.??proj"
    -- "src/**/*.shproj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) </> "bin/Release", "bin" </> (System.IO.Path.GetFileNameWithoutExtension f)))
    |>  Seq.iter (fun (fromDir, toDir) -> CopyDir toDir fromDir (fun _ -> true))
)

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" (fun _ ->
    CleanDirs ["bin"; "temp"]
)

// --------------------------------------------------------------------------------------
// Build library & test project

Target "Build" (fun _ ->
    !! solutionFile
#if MONO
    |> MSBuildReleaseExt "" [ ("DefineConstants","MONO") ] "Rebuild"
#else
    |> MSBuildRelease "" "Rebuild"
#endif
    |> ignore
)

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "ApplyLicenseHeader"
  ==> "Build"
  ==> "CopyBinaries"
  ==> "All"

RunTargetOrDefault "All"
