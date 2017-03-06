function hideMovieDisplayDetails() {
    $('#movieImage').hide();
    $('#pricingContainer').hide();
};

$(document).ready(function () {
    // Focus the search text box on page load.
    $("#movieSearch").focus();
    $('#movieSearch').css('width', '800px');

    // Hide the movie details until they are needed.
    hideMovieDisplayDetails();

    // Setup autocomplete for the movie search textbox.
    $("#movieSearch").autocomplete({
        minLength: 2,
        //source: baseUrl + "/Compare/GetMoviesSummary",  // Synchronous
        source: function (request, response) {
            getMoviesSummary(request, response);
        },
        focus: function (event, ui) {
            //$("#movieSearch").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            displayMovieDetails(ui.item);
            return false;
        }
    });
});

function getMoviesSummary(request, response) {
    var baseUrl = window.location.protocol + "//" + window.location.host;  // Could just use '@Url.Action("GetMoviesSummary")'
    var term = request;

    $.ajax({
        type: "GET",
        url: baseUrl + "/Compare/GetMoviesSummary",
        data: term,
        timeout: 10000,
        success: function (data) {
            hideMovieDisplayDetails();
            var jsonObj = JSON.parse(data);
            response($.map(jsonObj, function (value, key) {
                return {
                    value: value.title,
                    id: value.id,
                    year: value.year,
                    poster: value.poster
                };
            }));
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#message").text("Failed to retrieve the list of movie names (status of " + XMLHttpRequest.status + ")");
        }
    });
}

function displayMovieDetails(movie) {
    // Clear previous data
    $("#provider1").text("");
    $("#price1").text("");
    $("#provider2").text("");
    $("#price2").text("");
    $("#message").text("");

    // Display summary data
    $("#movieTitle").text(movie.value);
    $("#movieYear").text(movie.year);
    $("#movieImage").attr("src", movie.poster);

    $('#pricingContainer').show();
    $('#movieImage').show();
    $("#movieSearch").val("");

    var baseUrl = window.location.protocol + "//" + window.location.host;

    $.ajax({
        type: "GET",
        url: baseUrl + "/Compare/GetMoviePrices",
        data: {
            title: movie.value
        },
        timeout: 10000,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var jsonObj = JSON.parse(data);

            if (jsonObj[0]) {
                $("#provider1").text(jsonObj[0].provider);
                $("#price1").text("$" + jsonObj[0].price);
            }
            if (jsonObj[1]) {
                $("#provider2").text(jsonObj[1].provider);
                $("#price2").text("$" + jsonObj[1].price);
            }
            if (!jsonObj[0] && !(jsonObj[1])) {
                $("#message").text("No pricing was found at this time.");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#message").text("Failed to retrieve the movie prices (status of " + XMLHttpRequest.status + ")");
        }
    });
}