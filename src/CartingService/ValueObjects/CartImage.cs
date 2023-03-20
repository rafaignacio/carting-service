namespace CartingService.ValueObjects;

public class CartImage {
    public string URL { get; set; }
    public string AltText { get; set; }

    public CartImage (string url, string altText) {
        URL = url;
        AltText = altText;
    }

    public CartImage() {}
}