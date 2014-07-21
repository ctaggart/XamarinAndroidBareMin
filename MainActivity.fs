namespace BareMin

open System
open Android.App
open Android.Widget

[<Activity (Label = "BareMin", MainLauncher = true)>]
type MainActivity () =
    inherit Activity ()

    let mutable count = 1

    override this.OnCreate bundle =
        base.OnCreate bundle

        let layout = new LinearLayout(this)
        this.SetContentView layout
        layout.Orientation <- Orientation.Vertical

        let button = new Button(this)
        button.Text <- "touch me"
        layout.AddView button
        button.Click.Add (fun args -> 
            button.Text <- sprintf "%d touches" count
            count <- count + 1
        )
