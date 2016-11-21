var colors = ["#FF6384", "#4BC0C0", "#FFCE56", "#E7E9ED", "#36A2EB", "#F38630", "#E0E4CC", "#69D2E7", "#F7464A", "#E2EAE9", "#D4CCC5", "#949FB1", "#4D5360"];

function getChartColors(len) {
    if (len >= colors.length) {
        return colors;
    }
    return colors.slice(0, len);
}

function getChartColor(i) {
    if (i >= colors.length) {
        return colors[i % colors.length];
    }
    return colors[i];
}

function getChartColors2(len) {
    if (len >= colors.length) {
        var newColor = new Array();
        for (var i = 0; i < len; i++) {
            newColor[i] = getChartColor(i);
        }
        return newColor;
    }
    return colors;
}
