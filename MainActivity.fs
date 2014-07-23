namespace BareMin

open System
open Android.App
open Android.Views
open Android.Graphics

// my phone has a Canvas of 480 x 690
// (0,0) is the upper left hand corner

type CompleteGraph(ctx) =
    inherit View(ctx)

    let calcPoints width height n =
        let w = float32 width
        let h = float32 height
        let d = if w < h then w else h
        let r = d / 2.f

        let point i = 
            let t = float32 (i % n) / float32 n * 2.f * float32 Math.PI
            (r * sin t) + (w / 2.f), (r * cos t) + (h / 2.f)
        
        seq {
            for i = 0 to n - 1 do
                yield point i
        }

    let calcLines width height n =
        let points = calcPoints width height n |> Array.ofSeq
        seq {
            for i = 0 to n - 1 do
                for j = i + 1 to n - 1 do
                    yield points.[i], points.[j]
        }

    let p = new Paint()
    do
        p.Color <- Color.Purple

    let mutable numberOfPoints = 3

    override x.OnDraw canvas =
        canvas.DrawColor Color.White
        for (a,b),(c,d) in calcLines canvas.Width canvas.Height numberOfPoints do
            canvas.DrawLine(a, b, c, d, p)

    override x.OnTouchEvent ev =
        numberOfPoints <- numberOfPoints + 1
        x.Invalidate()
        false

    member x.Reset() =
        numberOfPoints <- 3
        x.Invalidate()

[<Activity (Label = "Complete Graph", MainLauncher = true)>]
type MainActivity() =
    inherit Activity()

    let mutable cg = Unchecked.defaultof<CompleteGraph>

    override x.OnCreate bundle =
        base.OnCreate bundle
        cg <- new CompleteGraph(x)
        x.SetContentView cg

    override x.OnBackPressed() =
        cg.Reset()