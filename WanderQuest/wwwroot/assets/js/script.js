// Navbar toggle
const hamburger = document.getElementById('hamburger');
const navMenu = document.getElementById('navMenu');
if (hamburger && navMenu) {
    hamburger.addEventListener('click', () => {
        navMenu.classList.toggle('active');
    });
}

// Hero Slider
const slides = document.querySelectorAll('#heroSlider .slide');
let currentSlide = 0;
const nextBtn = document.getElementById('nextBtn');
const prevBtn = document.getElementById('prevBtn');

function showSlide(index) {
    slides.forEach((slide, i) => {
        slide.classList.remove('active', 'prev', 'next');
        if (i === index) {
            slide.classList.add('active');
        } else if (i === (index - 1 + slides.length) % slides.length) {
            slide.classList.add('prev');
        } else if (i === (index + 1) % slides.length) {
            slide.classList.add('next');
        }
    });
    currentSlide = index;
}

if (nextBtn) {
    nextBtn.addEventListener('click', () => {
        showSlide((currentSlide + 1) % slides.length);
        clearInterval(autoSlide);
    });
} 
else {
    console.log("nextBtn element not found - likely not on product page");
}

if (prevBtn) {
    prevBtn.addEventListener('click', () => {
        showSlide((currentSlide - 1 + slides.length) % slides.length);
        clearInterval(autoSlide);
    });
}
else {
    console.log("prevBtn element not found - likely not on product page");
}




let autoSlide = setInterval(() => {
    showSlide((currentSlide + 1) % slides.length);
}, 5000);

const slider = document.getElementById('heroSlider');

if (slider) {
    slider.addEventListener('mouseenter', () => clearInterval(autoSlide));
    slider.addEventListener('mouseleave', () => {
        autoSlide = setInterval(() => {
            showSlide((currentSlide + 1) % slides.length);
        }, 5000);
    });
}
else {
    console.log("slider element not found - likely not on product page");
}

showSlide(currentSlide);













//Products
const packageGrid = document.getElementById('products');
if (packageGrid) {
    function renderPackages(filter = 'all') {
        const packageCards = packageGrid.querySelectorAll('.package-card');
        packageCards.forEach(card => {
            const category = card.dataset.id;
            if (filter === 'all' || category === filter) {
                card.classList.remove('hidden');
                if (filter === 'all') {
                    moreProductButton.classList.remove('hidden');
                }
            } else {
                card.classList.add('hidden');
                moreProductButton.classList.add('hidden');
            }
        });
    }
}

const filterBtns = document.querySelectorAll('.filter-btn');
if (filterBtns.length > 0) {
    filterBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            filterBtns.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            renderPackages(btn.dataset.filter);
        });
    });
}


//LoadMore
let loadMoreButton = document.getElementById("loadMore");
let products = document.getElementById("products");
if (loadMoreButton && products) {
    loadMoreButton.addEventListener("click", async function () {
        let productItems = document.getElementsByClassName("productItem");
        let skip = productItems.length;
        let resp = await fetch(`/products/loadmore/${skip}`);

        let data = await resp.text();
        if (data.trim() === "") {
            loadMoreButton.remove();
        }

        products.innerHTML += data;
    });
} else {
    console.log("loadMoreButton or products element not found - likely not on product page");
}


const testimonials = document.querySelectorAll('#testimonialSlider .testimonial');
let currentTestimonial = 0;
function showTestimonial(index) {
    testimonials.forEach((t, i) => {
        t.classList.remove('active', 'prev');
        if (i === index) t.classList.add('active');
        else if (i === (index - 1 + testimonials.length) % testimonials.length) t.classList.add('prev');
    });
    currentTestimonial = index;
}
setInterval(() => {
    showTestimonial((currentTestimonial + 1) % testimonials.length);
}, 4000);

const subscribeBtn = document.getElementById('subscribeBtn');
if (subscribeBtn) {
    subscribeBtn.addEventListener('click', () => {
        const email = document.getElementById('emailNewsletter').value;
        if (email) alert('Subscribed: ' + email);
    });
}




const productContainer = document.getElementById("products");
const basketCounts = document.querySelectorAll(".basketCount"); 

if (productContainer && basketCounts.length > 0) {
    productContainer.addEventListener("click", async function (e) {
        const addButton = e.target.closest(".addBasket");
        if (addButton) {
            let dataValue = addButton.getAttribute("data-value");
            try {
                let resp = await fetch("/products/SetBasket/" + dataValue);
                let data = await resp.json();

                if (data.status === 200) {
                    let totalCount = data.data.reduce((sum, item) => sum + item.count, 0);

                    // Tüm basketCount'lara yaz
                    basketCounts.forEach(el => {
                        el.innerHTML = ` ${totalCount}`;
                    });
                } else {
                    basketCounts.forEach(el => {
                        el.innerHTML = " 0";
                    });
                }
            } catch (err) {
                console.error("Wishlist update error:", err);
                basketCounts.forEach(el => {
                    el.innerHTML = " 0";
                });
            }
        }
    });
}

document.addEventListener("DOMContentLoaded", async function () {
    let basketCounts = document.querySelectorAll(".basketCount"); 

    if (basketCounts.length > 0) {
        try {
            let resp = await fetch("/products/getbasket");
            let data = await resp.json();

            if (data.status === 200) {
                let totalCount = 0;
                for (let item of data.data) {
                    totalCount += item.count;
                }

                basketCounts.forEach(el => {
                    el.innerHTML = ` ${totalCount}`;
                });
            } else {
                basketCounts.forEach(el => {
                    el.innerHTML = " 0";
                });
            }
        } catch (err) {
            console.error("Wishlist error:", err);
            basketCounts.forEach(el => {
                el.innerHTML = "Wishlist 0";
            });
        }
    }
});


const contactForm = document.getElementById('contactForm');
if (contactForm) {
    contactForm.addEventListener('submit', e => {
        e.preventDefault();
        const name = document.getElementById('name').value;
        const email = document.getElementById('email').value;
        alert(`Thank you, ${name}! We received your message.`);
        e.target.reset();
    });
}