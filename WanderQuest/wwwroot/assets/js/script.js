// Navbar toggle
const hamburger = document.getElementById('hamburger');
const navMenu = document.getElementById('navMenu');
if (hamburger && navMenu) {
    hamburger.addEventListener('click', () => {
        navMenu.classList.toggle('active');
    });
}



// Change product Count


async function setupBasketQuantityEvents() {

    document.querySelectorAll(".qty-decrease").forEach(button => {
        button.addEventListener("click", async () => {
            const productId = button.getAttribute("data-product-id");
            const input = button.closest(".wish-qty-price").querySelector(".wish-qty");
            let count = parseInt(input.value) - 1;

            if (count > 0) {
                input.value = count;
                await fetch(`/Basket/DecreaseProductQuantity/${productId}`, { method: 'POST' })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                            document.querySelector('#wishlistCount').textContent = data.totalItems;
                        }
                    });
                await loadBasketHover();
                await updateBasketTotals();
                await loadBasketProducts();
            } else {
                await fetch(`/Basket/DeleteProduct/${productId}`, { method: 'POST' })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            button.closest("li").remove();
                            document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                            document.querySelector('#wishlistCount').textContent = data.totalItems;
                            if (!document.querySelector(".wish-items li")) {
                                document.querySelector('.wish-items').innerHTML = '<li style="padding: 10px; text-align: center; color: #777;">Wishlist is empty</li>';
                            }
                        }
                    });
                await productMessage('Removed succesfully')
                await loadBasketHover();
                await updateBasketTotals();
                await loadBasketProducts();
            }
        });
    });

    document.querySelectorAll(".qty-increase").forEach(button => {
        button.addEventListener("click", async () => {
            const productId = button.getAttribute("data-product-id");
            const input = button.closest(".wish-qty-price").querySelector(".wish-qty");
            let count = parseInt(input.value) + 1;

            input.value = count;
            await fetch(`/Basket/IncreaseProductQuantity/${productId}`, { method: 'POST' })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                        document.querySelector('#wishlistCount').textContent = data.totalItems;
                    }
                });
            await loadBasketHover();
            await updateBasketTotals();
            await loadBasketProducts();
        });
    });

    document.querySelectorAll(".wish-qty").forEach(input => {
        input.addEventListener("change", async () => {
            const productId = input.getAttribute("data-product-id");
            let count = parseInt(input.value);

            if (count >= 1 && count <= 100) {
                await fetch(`/Basket/UpdateProductQuantity/${productId}/${count}`, { method: 'POST' })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                            document.querySelector('#wishlistCount').textContent = data.totalItems;
                        }
                    });
                await loadBasketHover();
                await updateBasketTotals();
                await loadBasketProducts();

            } else if (count < 1) {
                await fetch(`/Basket/DeleteProduct/${productId}`, { method: 'POST' })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            input.closest("li").remove();
                            document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                            document.querySelector('#wishlistCount').textContent = data.totalItems;
                            if (!document.querySelector(".wish-items li")) {
                                document.querySelector('.wish-items').innerHTML = '<li style="padding: 10px; text-align: center; color: #777;">Wishlist is empty</li>';
                            }
                        }
                    });
                await productMessage('Removed succesfully')
                await loadBasketHover();
                await updateBasketTotals();
                await loadBasketProducts();

            } else {
                input.value = 100;
                await fetch(`/Basket/UpdateProductQuantity/${productId}/100`, { method: 'POST' })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                            document.querySelector('#wishlistCount').textContent = data.totalItems;
                        }
                    });
                await loadBasketHover();
                await updateBasketTotals();
                await loadBasketProducts();

            }
        });
    });

    document.querySelectorAll(".delete-button").forEach(button => {
        button.addEventListener("click", async () => {
            const productId = button.getAttribute("data-product-id");

            await fetch(`/Basket/DeleteProduct/${productId}`, { method: 'POST' })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        button.closest("li").remove();
                        document.querySelector('.wish-total').textContent = `Total: $${data.totalPrice}`;
                        document.querySelector('#wishlistCount').textContent = data.totalItems;
                        if (!document.querySelector(".wish-items li")) {
                            document.querySelector('.wish-items').innerHTML = '<li style="padding: 10px; text-align: center; color: #777;">Wishlist is empty</li>';
                        }
                    }
                });

            await productMessage('Removed succesfully')
            await loadBasketHover();
            await updateBasketTotals();
            await loadBasketProducts();

        });
    });
}

document.addEventListener("DOMContentLoaded", async function () {
    await setupBasketQuantityEvents();
});






//Load Total Price and Total Count
async function updateBasketTotals() {
    try {
        const res = await fetch('/Basket/TotalBasketItemsInformation', { method: 'GET' });
        const data = await res.json();

        const totalPriceElements = document.querySelectorAll('.basketTotalPrice');
        const totalCountElements = document.querySelectorAll('.basketTotalCount');

        if (data.data) {
            totalPriceElements.forEach(el => el.textContent = `${data.data.totalPrice}`);
            totalCountElements.forEach(el => el.textContent = data.data.totalCount);
        } else {
            totalPriceElements.forEach(el => el.textContent = '0');
            totalCountElements.forEach(el => el.textContent = '0');
        }
    } catch (error) {
        console.error('Fetch hatası:', error);
    }
}
document.addEventListener("DOMContentLoaded", updateBasketTotals);




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

//Background Color Effect
function changeBackgroundColor(id = 0) {
    // package-card içinde data-product-id ile eşleşen id'yi bul
    const packageCards = document.querySelectorAll('.package-card[data-product-id]');
    packageCards.forEach(element => {
        if (element.getAttribute('data-product-id') === id.toString()) {
            element.style.backgroundColor = 'black'; // Bozartı ton
            setTimeout(() => {
                element.style.backgroundColor = 'white';
            }, 1000);
        }
    });

    // wishlist-items içindeki li içinde data-product-id ile eşleşen id'yi bul
    const wishlistItems = document.querySelectorAll('.wishlist-items li[data-product-id]');
    wishlistItems.forEach(element => {
        if (element.getAttribute('data-product-id') === id.toString()) {
            element.style.backgroundColor = 'black'; // Bozartı ton
            setTimeout(() => {
                element.style.backgroundColor = 'white';
            }, 1000);
        }
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
        await changeBackgroundColor();
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







//Product Message
function productMessage(message = 'Added to cart') {
    // Yeni bir div oluştur
    const messageBox = document.createElement('div');
    messageBox.className = 'message-box';
    messageBox.innerHTML = message;

    // DOM'a ekle (document.body yerine güvenli bir kontrol)
    const target = document.body || document.documentElement;
    target.appendChild(messageBox);

    // .wishlist-dropdown'a stilleri ekle
    const dropdown = document.querySelector('.wishlist-dropdown');
    if (dropdown) {
        dropdown.style.opacity = '1';
        dropdown.style.visibility = 'visible';
        dropdown.style.transform = 'translateY(0)';
    }

    // 2 saniye sonra her iki elementi de sil
    setTimeout(() => {
        messageBox.remove();
        if (dropdown) {
            dropdown.style.opacity = '0';
            dropdown.style.visibility = 'hidden';
            dropdown.style.transform = 'translateY(-10px)';
        }
    }, 2000);
}


//document.addEventListener("DOMContentLoaded", async function () {
//    const productContainer = document.getElementById("products");
//    const basketCounts = document.querySelectorAll(".basketCount");

//    if (productContainer && basketCounts.length > 0) {
//        productContainer.addEventListener("click", async function (e) {
//            const addButton = e.target.closest(".addBasket");
//            if (addButton) {
//                let dataValue = addButton.getAttribute("data-value");
//                try {
//                    let resp = await fetch("/products/SetBasket/" + dataValue);
//                    await addProduct();
//                    Console.log("yazilmald");
//                    await updateBasketTotals();
//                    let data = await resp.json();

//                    if (data.status === 200) {
//                        let totalCount = data.data.reduce((sum, item) => sum + item.count, 0);

//                        // Tüm basketCount'lara yaz
//                        basketCounts.forEach(el => {
//                            el.innerHTML = ` ${totalCount}`;
//                        });
//                    } else {
//                        basketCounts.forEach(el => {
//                            el.innerHTML = " 0";
//                        });
//                    }
//                } catch (err) {
//                    console.error("Wishlist update error:", err);
//                    basketCounts.forEach(el => {
//                        el.innerHTML = " 0";
//                    });
//                    await addProduct();
//                    await updateBasketTotals();
//                }
//            }
//        });
//    }
//});

//document.addEventListener("DOMContentLoaded", async function () {
//    let basketCounts = document.querySelectorAll(".basketCount");

//    if (basketCounts.length > 0) {
//        try {
//            let resp = await fetch("/products/getbasket");
//            let data = await resp.json();
//            await updateBasketTotals();

//            if (data.status === 200) {
//                let totalCount = 0;
//                for (let item of data.data) {
//                    totalCount += item.count;
//                }

//                basketCounts.forEach(el => {
//                    el.innerHTML = ` ${totalCount}`;
//                });
//            } else {
//                basketCounts.forEach(el => {
//                    el.innerHTML = " 0";
//                });
//            }
//        } catch (err) {
//            console.error("Wishlist error:", err);
//            basketCounts.forEach(el => {
//                el.innerHTML = "Wishlist 0";
//            });
//            await updateBasketTotals();
//        }
//    }
//});



async function loadBasketHover() {
    // Basket Hover And Add Change
    // 🌀 Basket-in hover görünüşünü serverdən yükləyir
    try {
        const res = await fetch('/Basket/GetHoverDetailsHtml');
        if (!res.ok) throw new Error('Server error');

        const data = await res.json();

        const container = document.getElementById('hoverContainer');
        if (container) {
            container.innerHTML = data.html;
            await changeBackgroundColor();
            await setupBasketQuantityEvents();

        } else {
            console.warn('hoverContainer tapılmadı');
        }
    } catch (error) {
        console.error('Load basket hover error:', error);
    }
}

// 🖱️ addBasket düyməsinə klik olduqda basketə əlavə edir və hover-i yeniləyir
document.addEventListener('click', async (e) => {
    const addButton = e.target.closest('.addBasket');
    if (!addButton) return;

    const dataValue = addButton.getAttribute('data-value');
    if (!dataValue) {
        console.warn('data-value tapılmadı');
        return;
    }

    try {
        const resp = await fetch('/products/SetBasket/' + dataValue);
        const data = await resp.json();

        if (data.status === 200) {
            // ✅ Uğurla əlavə olundu — hover görünüşü yenilənsin
            //
            //
            //
            //console.log("fetch atilir");
            await changeBackgroundColor(dataValue);
            await productMessage();
            await loadBasketHover();
            await updateBasketTotals();
            await loadBasketProducts();
        } else {
            console.error('Basket update failed');
        }
    } catch (err) {
        console.error('Add to basket error:', err);
    }
});

// 📦 Səhifə açıldıqda ilk basket hover yüklə
document.addEventListener('DOMContentLoaded', async () => {
    await loadBasketHover();
});












//Get Basket Products
async function loadBasketProducts() {
    try {
        const res = await fetch('/Basket/GetBasketProductsHtml');
        if (!res.ok) throw new Error('Server error');

        const data = await res.json();

        const container = document.getElementById('basketProductsContainer');
        if (container) {
            container.innerHTML = data.html;
            await changeBackgroundColor();
            await setupBasketQuantityEvents();
            await updateBasketTotals();

        } else {
            console.warn('basketProductsContainer tapılmadı');
        }
    } catch (error) {
        console.error('Load basket hover error:', error);
    }
}
document.addEventListener('click', async (e) => {
    const addButton = e.target.closest('.addBasket');
    if (!addButton) return;

    const dataValue = addButton.getAttribute('data-value');
    if (!dataValue) {
        console.warn('data-value tapılmadı');
        return;
    }

    try {
        const resp = await fetch('/products/SetBasket/' + dataValue);
        const data = await resp.json();

        if (data.status === 200) {
            // ✅ Uğurla əlavə olundu — hover görünüşü yenilənsin
            //
            //
            //
            await changeBackgroundColor(dataValue);
            await loadBasketProducts();
            await updateBasketTotals();
        } else {
            console.error('Basket update failed');
        }
    } catch (err) {
        console.error('Add to basket error:', err);
    }
});

// 📦 Səhifə açıldıqda ilk basket hover yüklə
document.addEventListener('DOMContentLoaded', async () => {
    await loadBasketProducts();
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