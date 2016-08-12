// Copyright (c) University of Warwick. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace HighFieldEpr

open System
open FsXaml

module App =
    type App = XAML<"App.xaml">

    [<STAThread>]
    [<EntryPoint>]
    let main _ =
        App().Run()
