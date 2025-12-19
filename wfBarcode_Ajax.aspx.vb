Public Class wfBarcode_Ajax
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Get the Requested code to be created.
        Dim Code As String = Request.QueryString("Barcode")

        ' Multiply the lenght of the code by 15 (just to have enough width)
        Dim w As Integer = Code.Length * 16

        ' Create a bitmap object of the width that we calculated and height of 35
        Dim oBitmap As New Bitmap(w, 30)

        ' then create a Graphic object for the bitmap we just created.
        Dim oGraphics As Graphics = Graphics.FromImage(oBitmap)

        ' Now create a Font object for the Barcode Font
        ' (in this case the FRE3OF9X_0) of 18 point size
        Dim oFont As New Font("Free 3 of 9 Extended", 25)

        ' Let's create the Point and Brushes for the barcode
        Dim oPoint As New PointF(2.0F, 2.0F)
        Dim oBrushWrite As New SolidBrush(Color.Black)
        Dim oBrush As New SolidBrush(Color.White)

        ' Now lets create the actual barcode image
        ' with a rectangle filled with white color
        oGraphics.FillRectangle(oBrush, 0, 0, w, 30)

        ' We have to put prefix and sufix of an asterisk (*),
        ' in order to be a valid barcode
        oGraphics.DrawString((Convert.ToString("*") & Code) + "*", oFont, oBrushWrite, oPoint)

        ' Then we send the Graphics with the actual barcode
        Response.ContentType = "image/jpeg"
        oBitmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
    End Sub

End Class