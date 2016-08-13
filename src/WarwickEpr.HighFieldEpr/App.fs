// Copyright (c) University of Warwick. All Rights Reserved. Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace HighFieldEpr

open System
open FsXaml

module App =
    type AppBase = XAML<"App.xaml">
    
    type App() =
        inherit AppBase()

        member app.Application_Startup(sender : obj, e : System.Windows.StartupEventArgs) =
            MainWindow().Show()

    [<STAThread>]
    [<EntryPoint>]
    let main _ =
        App().Run()
