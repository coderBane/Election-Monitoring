window.sliderFunctions = {
    candidateSpeaks : function() {
        let slider = tns({
            container: '.candidate-speaks',
            items: 1,
            slideBy: 'page',
            loop: true,
            autoplay: true,
            autoplayButtonOutput: false,
            autoplayHoverPause: true,
            arrowKeys: true,
            controls: false,
            speed: 1000,
            responsive: {
                600: { items: 2, gutter: 15, mouseDrag: true},
                800: { items: 3, gutter: 35}
            }
        });
    }
}
