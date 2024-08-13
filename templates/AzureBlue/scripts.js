$(document).ready(function() {
    var url = new URL(window.location.href);
    var tenant = "";
    var stylesheet = "";
    if (url.searchParams.has("tenantId")) {
        tenant = url.searchParams.get("tenantId");
        if (tenant === "67524f15-f870-440d-a2d3-075264371001") {
            tenant = "agilebridge";
            stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/platform.css";
        }
        else {
            tenant = "lightstone";
            stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/platform.css";
        }
    }
    else {
        tenant = "lightstone";
        stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/platform.css";
    }
    // var styleSheet = 'https://cdn.test.platform.coaxle.co.za/tenantmanagement-api-test/styles/tenant.css?tenantId=' + tenantId;
    document.getElementsByTagName("head")[0].insertAdjacentHTML(
        "beforeend",
        "<link rel='stylesheet' href='" + stylesheet + "' />");
});