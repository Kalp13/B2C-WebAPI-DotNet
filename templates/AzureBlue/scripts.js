$(document).ready(function() {
    var url = new URL(window.location.href);
    var tenant = "";
    var stylesheet = "";
    if (url.searchParams.has("client_id")) {
        tenant = url.searchParams.get("client_id");
        if (tenant === "67524f15-f870-440d-a2d3-075264371001") {
            tenant = "agilebridge";
            stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/agilebridge/abstyle.css";
        }
        else{
            tenant = "lightstone";
            stylesheet = "https://ssotestsarfk.blob.core.windows.net/styles/lightstone/lsstyle.css";
        }
    }
    else if (url.searchParams.has("tenant")) {
        tenant = url.searchParams.get("tenant");
    }
    else {
        tenant = "default";
    }
    // var styleSheet = 'https://cdn.test.platform.coaxle.co.za/tenantmanagement-api-test/styles/tenant.css?tenantId=' + tenantId;
    document.getElementsByTagName("head")[0].insertAdjacentHTML(
        "beforeend",
        "<link rel='stylesheet' href='" + stylesheet + "' />");
});