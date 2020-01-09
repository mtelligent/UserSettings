XA.component.preferencesList = (function ($, document) {

    function init() {

        $('.preferences-list-item').click(function (event) {

            event.preventDefault();

            var valueToSet = $(this).attr("data-value");
            var targetUrl = $(this).attr("data-targetUrl");
            var container = $(this).parent();
            var area = container.attr("data-area");
            var key = container.attr("data-key");

            setPreference(area, key, valueToSet, targetUrl);
        });

        function setPreference(area, key, valueToSet, targetUrl) {
            var data = "=" + valueToSet;
            var url = "/api/sf/1.0/userSettings/" + area + "/" + key;
            $.post(url, data).done(function () {
                if (targetUrl && targetUrl.length > 0) {
                    window.location.href = targetUrl;
                }
                else {
                    $(".preferences-list-container").hide();
                    $(".preferences-list-thank-you-message").show();
                }
            }).fail(function(){
                console.log("error saving preference");
            });
        }

    }


    var pub = {};

    pub.init = function () {
        init();
    };

    return pub;

}(jQuery, document));

XA.register("preferencesList", XA.component.preferencesList);