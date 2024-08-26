function initializeSwiper(swiperSelector, paginationSelector, nextButtonSelector, prevButtonSelector) {
    new Swiper(swiperSelector, {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        autoplay: {
            delay: 2500,
            disableOnInteraction: false,
        },
        pagination: {
            el: paginationSelector,
            clickable: true,
        },
        navigation: {
            nextEl: nextButtonSelector,
            prevEl: prevButtonSelector,
        },
        // Responsive breakpoints
        breakpoints: {
            // Extra small devices (phones, 600px and down)
            600: {
                slidesPerView: 1,
                spaceBetween: 10
            },
            // Small devices (portrait tablets and large phones, 600px to 768px)
            768: {
                slidesPerView: 1,
                spaceBetween: 20
            },
            // Medium devices (landscape tablets, 768px to 992px)
            992: {
                slidesPerView: 2,
                spaceBetween: 30
            },
            // Large devices (laptops/desktops, 992px to 1200px)
            1200: {
                slidesPerView: 3,
                spaceBetween: 40
            },
            // Extra large devices (large laptops and desktops, 1200px and up)
            1200: {
                slidesPerView: 3,
                spaceBetween: 50
            }
        }
    });
}

// Initialize the gallery swiper
initializeSwiper('.gallery-swiper', '.gallery-swiper-pagination', '.gallery-swiper-button-next', '.gallery-swiper-button-prev');

// Initialize the news swiper
// Ensure you have similar HTML structure for news-swiper as you have for gallery-swiper
initializeSwiper('.news-swiper', '.news-swiper-pagination', '.news-swiper-button-next', '.news-swiper-button-prev');

// Initialize the event swiper
// Ensure you have similar HTML structure for event-swiper as you have for gallery-swiper
initializeSwiper('.event-swiper', '.event-swiper-pagination', '.event-swiper-button-next', '.event-swiper-button-prev');