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
        let max = if w < h then w else h
        
        let point i = 
            let t = float32 (i % n) / float32 n * max * float32 Math.PI
            w / 2.f + sin t, h / 2.f + cos t
        
        seq {
            for i = 0 to n - 1 do
                 yield point i
        }

    let calcLines width height n =
        let points = calcPoints width height n |> Array.ofSeq // do with seq?
        seq {
            yield points.[points.Length-1], points.[0]
            for i = 0 to points.Length - 2 do
                yield points.[i], points.[i+1]
        }

    let p = new Paint()
    do
        //p.StrokeWidth <- 6.f
        p.Color <- Color.Purple

    override x.OnDraw canvas =
        canvas.DrawColor Color.White

//        canvas.DrawLine(0.f,0.f, float32 canvas.Width, float32 canvas.Height, p)

        for (a,b),(c,d) in calcLines canvas.Width canvas.Height 3 do
            canvas.DrawLine(a, b, c, d, p)


[<Activity (Label = "CompleteGraph", MainLauncher = true)>]
type MainActivity() =
    inherit Activity()

    let mutable count = 0

    override this.OnCreate bundle =
        base.OnCreate bundle

        let v = new CompleteGraph(this)
        this.SetContentView v