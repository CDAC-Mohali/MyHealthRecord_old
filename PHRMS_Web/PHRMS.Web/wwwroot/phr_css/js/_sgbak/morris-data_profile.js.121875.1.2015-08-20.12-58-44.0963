$(function() {


    //Morris.Line({
    //    element: 'line-chart',
    //    data: [
    //      { y: '2012-10-18', a: 100, b: 90, c: 60 },
    //      { y: '2012-10-18', a: 102, b: 65, c: 65 },
    //      { y: '2012-10-18', a: 120, b: 40, c: 70 },
    //      { y: '2012-10-18', a: 115, b: 65, c: 80 },
    //      { y: '2012-10-18', a: 120, b: 40, c: 60 },
    //      { y: '2012-10-18', a: 110, b: 65, c: 72 },
    //      { y: '2012-10-18', a: 100, b: 90, c: 86 }
    //    ],
    //    xkey: 'y',
    //    ykeys: ['a', 'b', 'c'],
    //    lineColors: ['#7F0F65', '#63AB6F', '#528CAD'],
    //    labels: ['Systolic', 'Diastolic', 'Pulse']
    //});


    //Morris.Line({
    //    element: 'line-chart',
    //    data: [
    //                  { date: '2012-10-18', winners: 1, nonWinners: 0, total: 1 },
    //                  { date: '2012-10-19', winners: 0, nonWinners: 1, total: 1 },
    //                  { date: '2012-10-20', winners: 1, nonWinners: 0, total: 1 },
    //    ],
    //    xkey: 'date',
    //    ykeys: ['winners', 'nonWinners', 'total'],
    //    xLabelFormat: function (date) {
    //        return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
    //    },
    //    xLabels: 'day',
    //    labels: ['Winners', 'Non-winners', 'Total'],
    //    lineColors: ['#167f39', '#990000', '#000099'],
    //    lineWidth: 2,
    //    dateFormat: function (date) {
    //        d = new Date(date);
    //        return d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();
    //    },
    //});


    //Morris.Line({
    //    element: 'line-chartsystolic',
    //    data: [
    //      { date: '2015-07-08', Systolic: 110, Diastolic: 100, Pulse: 75 },
    //      { date: '2015-07-09', Systolic: 102, Diastolic: 95, Pulse: 65 },
    //      { date: '2015-07-10', Systolic: 120, Diastolic: 115, Pulse: 85 },

    //    ],
    //    xkey: 'date',
    //    ykeys: ['Systolic', 'Diastolic', 'Pulse'],
    //    xLabelFormat: function (date) {
    //        return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
    //    },
    //    xLabels: 'day',
    //    labels: ['Systolic', 'Diastolic', 'Pulse'],
    //    lineColors: ['#167f39', '#990000', '#000099'],
    //    lineWidth: 2,
    //    dateFormat: function (date) {
    //        d = new Date(date);
    //        return d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();
    //    },
    //});

    Morris.Line({
        element: 'line-chartsystolic',
        data: [
          { date: '2015-07-08', Systolic: 110 },
          { date: '2015-07-09', Systolic: 102 },
          { date: '2015-07-10', Systolic: 120},

        ],
        xkey: 'date',
        ykeys: ['Systolic'],
        xLabelFormat: function (date) {
            return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
        },
        xLabels: 'day',
        labels: ['Systolic'],
        lineColors: ['#167f39'],
        lineWidth: 2,
        dateFormat: function (date) {
            d = new Date(date);
            return d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();
        },
    });

    Morris.Line({
        element: 'line-chartdiastolic',
        data: [
          { date: '2015-07-08',  Diastolic: 100 },
          { date: '2015-07-09',  Diastolic: 95 },
          { date: '2015-07-10', Diastolic: 115 },

        ],
        xkey: 'date',
        ykeys: [ 'Diastolic'],
        xLabelFormat: function (date) {
            return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
        },
        xLabels: 'day',
        labels: ['Diastolic'],
        lineColors: ['#990000'],
        lineWidth: 2,
        dateFormat: function (date) {
            d = new Date(date);
            return d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();
        },
    });

    Morris.Line({
        element: 'line-chartpulse',
        data: [
          { date: '2015-07-08', Pulse: 75 },
          { date: '2015-07-09', Pulse: 65 },
          { date: '2015-07-10', Pulse: 85 },

        ],
        xkey: 'date',
        ykeys: ['Pulse'],
        xLabelFormat: function (date) {
            return date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
        },
        xLabels: 'day',
        labels: ['Pulse'],
        lineColors: [ '#000099'],
        lineWidth: 2,
        dateFormat: function (date) {
            d = new Date(date);
            return d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();
        },
    });
});


