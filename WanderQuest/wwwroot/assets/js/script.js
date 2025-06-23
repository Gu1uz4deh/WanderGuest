/* script.js */
// Navbar toggle
const hamburger = document.getElementById('hamburger');
const navMenu = document.getElementById('navMenu');
hamburger.addEventListener('click', () => {
    navMenu.classList.toggle('active');
});


let loadMoreButton = document.getElementById("loadMore");
let products = document.getElementById("products");

loadMoreButton.addEventListener("click", async function () {
    let resp = await fetch("/products/loadmore");
    let data = await resp.text();
    console.log(data);
    products.innerHTML += data;
})

// Hero Slider
const slides = document.querySelectorAll('#heroSlider .slide');
let currentSlide = 0;
const nextBtn = document.getElementById('nextBtn');
const prevBtn = document.getElementById('prevBtn');
function showSlide(index) {
    slides.forEach((slide, i) => {
        slide.classList.remove('active', 'prev');
        if (i === index) slide.classList.add('active');
        else if (i === (index - 1 + slides.length) % slides.length) slide.classList.add('prev');
    });
    currentSlide = index;
}
nextBtn.addEventListener('click', () => {
    showSlide((currentSlide + 1) % slides.length);
});
prevBtn.addEventListener('click', () => {
    showSlide((currentSlide - 1 + slides.length) % slides.length);
});
setInterval(() => {
    showSlide((currentSlide + 1) % slides.length);
}, 5000);

const packageGrid = document.getElementById('packageGrid');

//function renderPackages(filter = 'all') {
//    const packageCards = packageGrid.querySelectorAll('.package-card');
//    packageCards.forEach(card => {
//        const category = card.dataset.category;
//        if (filter === 'all' || category === filter) {
//            card.classList.remove('hidden');
//        } else {
//            card.classList.add('hidden');
//        }
//    });
//}

const filterBtns = document.querySelectorAll('.filter-btn');
filterBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        filterBtns.forEach(b => b.classList.remove('active'));
        btn.classList.add('active');
        renderPackages(btn.dataset.filter);
    });
});

//renderPackages();
let wishlistCount = 0;
document.addEventListener('click', e => {
    if (e.target.matches('.package-card-content button')) {
        wishlistCount++;
        document.getElementById('wishlistCount').textContent = wishlistCount;
        alert('Added to wishlist!');
    }
});

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

document.getElementById('subscribeBtn').addEventListener('click', () => {
    const email = document.getElementById('emailNewsletter').value;
    if (email) alert('Subscribed: ' + email);
});

const blogs = [
    { id: 1, title: 'Top 10 Mountain Trails', date: '01 June 2025', img: 'https://picsum.photos/200?random=1' },
    { id: 2, title: 'Beach Safety Tips', date: '15 May 2025', img: 'https://picsum.photos/200?random=2' },
    { id: 3, title: 'City Travel Hacks', date: '20 April 2025', img: 'https://picsum.photos/200?random=3' },
];
const blogGrid = document.getElementById('blogGrid');
blogs.forEach(b => {
    const card = document.createElement('div');
    card.className = 'blog-card';
    card.innerHTML = `
        <img src="${b.img}" alt="${b.title}">
        <div class="blog-card-content">
            <h3>${b.title}</h3>
            <p>${b.date}</p>
            <a href="#">Read More</a>
        </div>
    `;
    blogGrid.appendChild(card);
});


document.getElementById('contactForm').addEventListener('submit', e => {
    e.preventDefault();
    const name = document.getElementById('name').value;
    const email = document.getElementById('email').value;
    alert(`Thank you, ${name}! We received your message.`);
    e.target.reset();
});
