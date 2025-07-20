
//#region DOMContentLoaded
document.addEventListener("DOMContentLoaded", async function () {
    await setupBasketQuantityEvents();
    await updateBasketTotals();
    await loadBasketHover();
    await loadBasketProducts();
});
//#endregion

//#region Navbar toggle
const hamburger = document.getElementById('hamburger');
const navMenu = document.getElementById('navMenu');
if (hamburger && navMenu) {
    hamburger.addEventListener('click', () => {
        navMenu.classList.toggle('active');
    });
}
//#endregion

//#region Change product Count
async function setupBasketQuantityEvents() {

    document.querySelectorAll(".qty-decrease").forEach(button => {
        button.addEventListener("click", async () => {
            const productId = button.getAttribute("data-product-id");
            const input = button.closest(".wish-qty-price").querySelector(".wish-qty");
            let count = parseInt(input.value) - 1;

            if (count > 0) {
                input.value = count;
                await fetch(`/Basket/DecreaseProductQuantity/${productId}`);

            } else {
                await fetch(`/Basket/DecreaseProductQuantity/${productId}`);
                
                await productMessage('Removed succesfully');
            }
            await loadBasketHover();
            await updateBasketTotals();
            await loadBasketProducts();
        });
    });

    document.querySelectorAll(".qty-increase").forEach(button => {
        button.addEventListener("click", async () => {
            const productId = button.getAttribute("data-product-id");
            const input = button.closest(".wish-qty-price").querySelector(".wish-qty");
            let count = parseInt(input.value) + 1;

            input.value = count;
            await fetch(`/Basket/IncreaseProductQuantity/${productId}`);
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
                await fetch(`/Basket/UpdateProductQuantity/${productId}/${count}`);
                

            } else if (count < 1) {
                await fetch(`/Basket/DeleteProduct/${productId}`);
                
                await productMessage('Removed succesfully')

            } else {
                input.value = 100;
                await fetch(`/Basket/UpdateProductQuantity/${productId}/100`);
            }
            await loadBasketHover();
            await updateBasketTotals();
            await loadBasketProducts();
        });
    });

    document.querySelectorAll(".delete-button").forEach(button => {
        button.addEventListener("click", async () => {
            const productId = button.getAttribute("data-product-id");

            await fetch(`/Basket/DeleteProduct/${productId}`);

            await productMessage('Removed succesfully')
            await loadBasketHover();
            await updateBasketTotals();
            await loadBasketProducts();

        });
    });

}
//#endregion

//#region Update Basket Totals
async function updateBasketTotals() {
    const summary = await getBasketSummary();

    const priceElements = document.querySelectorAll('.basketTotalPrice');
    const countElements = document.querySelectorAll('.basketTotalCount');

    if (summary.totalItems === 0 && summary.totalPrice === 0) {
        priceElements.forEach(el => el.style.display = 'none');
        countElements.forEach(el => el.style.display = 'none');
    } else {
        priceElements.forEach(el => {
            el.style.display = 'inline-block';
            el.textContent = `$ ${summary.totalPrice}`;
        });
        countElements.forEach(el => {
            el.style.display = 'inline-block';
            el.textContent = summary.totalItems;
        });
    }
}
//#endregion

//#region Get Basket Summary
async function getBasketSummary() {
    const response = await fetch('/Basket/GetBasketSummary', {
        method: 'POST'
    });

    if (!response.ok) {
        throw new Error('Basket summary could not be fetched.');
    }

    const data = await response.json();
    return {
        totalPrice: data.totalPrice,
        totalItems: data.totalItems
    };
}
//#endregion

//#region Hero Slider
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
//#endregion

//#region Background Color Effect
function changeBackgroundColor(id = 0) {
    // package-card içinde data-product-id ile eşleşen id'yi bul
    const packageCards = document.querySelectorAll('.package-card[data-product-id]');
    packageCards.forEach(element => {
        if (element.getAttribute('data-product-id') === id.toString()) {
            element.style.backgroundColor = '#c4c4c4'; // Bozartı ton
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
//#endregion

//#region
const filterBtns = document.querySelectorAll('.filter-btn');
if (filterBtns.length > 0) {
    filterBtns.forEach(btn => {
        btn.addEventListener('click', async () => {
            filterBtns.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');

            const categoryId = btn.getAttribute('data-category-id');
            if (!categoryId) {
                console.warn('Category ID tapılmadı');
                return;
            }
            console.log(parseInt(categoryId));
            
        //await loadCategoryProducts(categoryId, 4);
            // categoryId string olur, amma serverdə int lazımdırsa, parse et:
            const container = document.querySelector('#products');
            container.innerHTML = ''; // Köhnə məhsulları təmizlə
            await loadCategoryProducts(parseInt(categoryId), 0, 1); // skip = 0 olaraq başlasın
        });
    });
}

let loadMoreButton = document.getElementById("loadMore");
let products = document.getElementById("products");
if (loadMoreButton && products) {
    loadMoreButton.addEventListener("click", async function () {

        let productItems = document.getElementsByClassName("productItem");
        let skip = productItems.length;
        console.log(skip);

        const active = document.querySelector('.active');
        const categoryId = active.getAttribute('data-category-id');
        console.log(parseInt(categoryId));
        await loadCategoryProducts(parseInt(categoryId), skip, 1); 
    });
} else {
    console.log("loadMoreButton or products element not found - likely not on product page");
}

async function loadCategoryProducts(categoryId, skip, take) {
    try {
        const res = await fetch(`/products/LoadCategoryProductsHtml?categoryId=${categoryId}&skip=${skip}&take=${take}`);
        //const data = await res.text(); 
        const data = await res.json();
        console.log(data);

        const container = document.querySelector('#products');
        container.innerHTML += data.html; 
    } catch (err) {
        console.error("Məhsullar yüklənərkən xəta:", err);
    }
}
//#endregion

//#region TestimonialSlider
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
//#endregion

//#region Product Message
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
//#endregion

//#region Load Basket Hover
async function loadBasketHover() {
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
//#endregion

//#region Click addBasket
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
//#endregion

//#region Get Basket Products
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

//#endregion

//#region Chat Connection Section
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();
connection.on("ReceiveMessage", function (senderId, messageText, sentAt) {
    const viewBagDiv = document.querySelector(".viewBagUsername");
    const myUserId = viewBagDiv.getAttribute("data-value");
    const isMyMessage = senderId === myUserId;
    
    const msgWrapper = document.createElement("div");
    msgWrapper.className = isMyMessage ? "message sent" : "message received";

    // İsteğe bağlı avatar için img etiketi eklenebilir
    // const avatar = document.createElement("img");
    // avatar.src = "https://via.placeholder.com/40";
    // avatar.alt = "User Avatar";
    // avatar.className = "avatar";
    // msgWrapper.appendChild(avatar);

    const messageContent = document.createElement("div");
    messageContent.className = "message-content";

    const messageTextP = document.createElement("p");
    messageTextP.textContent = messageText;

    const timeSpan = document.createElement("span");
    timeSpan.className = isMyMessage ? "timestamp-sent" : "timestamp-received";
    timeSpan.textContent = new Date(sentAt).toLocaleTimeString();

    messageContent.appendChild(messageTextP);
    messageContent.appendChild(timeSpan);
    msgWrapper.appendChild(messageContent);

    document.getElementById("messagesList").appendChild(msgWrapper);
    const container = document.getElementById("messagesList");
    container.scrollTop = container.scrollHeight;
    if (!isMyMessage) {
        showToast(senderId + " say: " + messageText)
    }
});
connection.start();

function showToast(message) {
    const toast = document.getElementById("toast");
    toast.textContent = message;
    toast.className = "toast show";
    setTimeout(() => {
        toast.className = "toast";
    }, 2000); // 2 saniye sonra kapanır
}
//#endregion

//#region LoadChatMessages
function loadMessages(userId) {
    document.getElementById("receiverId").value = userId;

    fetch(`/Chat/GetMessages?receiverId=${userId}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById("messagesList").innerHTML = html;
            showChatSection();
            // YALNIZCA buraya koyarsan çalışır: HTML yüklendikten sonra
            const container = document.getElementById("messagesList");
            container.scrollTop = container.scrollHeight;

            const chatHeader = document.getElementById("chatHeader");
            chatHeader.innerHTML = `<h2>${userId}</h2>`;
        });
}
//#endregion

//#region SendMessage
function sendMessage() {
    const receiverId = document.getElementById("receiverId").value;
    const message = document.getElementById("messageInput").value;

    if (!receiverId || !message) return;

    connection.invoke("SendMessage", receiverId, message).catch(console.error);
    document.getElementById("messageInput").value = "";
}
//#endregion

//#region Search User
function searchUsers() {
    const query = document.getElementById("userSearchInput").value;
    if (query.length < 2) return;

    fetch(`/Chat/SearchUser?query=${encodeURIComponent(query)}`)
        .then(res => res.json())
        .then(data => {
            const list = document.getElementById("userList");
            list.innerHTML = "";

            data.forEach(user => {
                const div = document.createElement("div");
                div.className = "user-item";
                div.onclick = () => loadMessages(user.userName);

                //div.setAttribute("onclick", `loadMessages('${user.userName}')`);

                // Avatar
                const img = document.createElement("img");
                img.src = "https://via.placeholder.com/40";
                img.alt = "User Avatar";
                img.className = "avatar";
                div.appendChild(img);

                // User info container
                const userInfo = document.createElement("div");
                userInfo.className = "user-info";

                // User name
                const h3 = document.createElement("h3");
                h3.className = "user-name";
                h3.textContent = user.userName; // veya user.name, api'ye göre değişebilir
                userInfo.appendChild(h3);

                // Last message (statik örnek)
                const p = document.createElement("p");
                p.className = "last-message";
                p.textContent = "";
                userInfo.appendChild(p);

                div.appendChild(userInfo);

                // Timestamp (statik örnek)
                const span = document.createElement("span");
                span.className = "timestamp";
                span.textContent = "";
                div.appendChild(span);

                list.appendChild(div);
            });
        });
}
document.addEventListener('DOMContentLoaded', function () {
    const searchBar = document.querySelector('.search-bar');
    const searchDropdown = document.querySelector('.search-dropdown');
    const trashIcon = document.querySelector('.trash-icon');

    // Arama çubuğuna tıklayınca dropdown aç
    searchBar.addEventListener('focus', function () {
        searchDropdown.style.display = 'block';
    });

    // Başka yere tıklayınca dropdown kapat
    document.addEventListener('click', function (event) {
        if (!searchBar.contains(event.target) && !searchDropdown.contains(event.target)) {
            searchDropdown.style.display = 'none';
        }
    });

    // Çöp ikonuna tıklayınca arama çubuğunu temizle
    trashIcon.addEventListener('click', function () {
        searchBar.value = '';
    });
})
//#endregion

//#region Change Chat Section
function showEmptySection() {
    const chatSection = document.querySelector('.right-section');
    const emptySection = document.querySelector('.right-section-empty');

    if (chatSection) chatSection.style.setProperty("display", "none", "important");
    if (emptySection) emptySection.style.setProperty("display", "flex", "important");
}

function showChatSection() {
    const chatSection = document.querySelector('.right-section');
    const emptySection = document.querySelector('.right-section-empty');

    if (emptySection) emptySection.style.setProperty("display", "none", "important");
    if (chatSection) chatSection.style.setProperty("display", "flex", "important");
}


document.addEventListener("DOMContentLoaded", function () {
    showEmptySection();
});

//#endregion

//#region Delete CHATBTN
document.getElementById("deleteAllBtn").addEventListener("click", async () => {
    const userId = document.getElementById("receiverId").value;
    if (!userId) {
        alert("No user selected to delete messages!");
        return;
    }

    // Onay kutusu
    const confirmed = confirm(`Are you sure you want to delete all messages with ${userId}?`);
    if (!confirmed) return; // Kullanıcı hayır dedi, işlem iptal

    try {
        const response = await fetch(`/deleteAllMessages/${encodeURIComponent(userId)}`, {
            method: "DELETE",
        });

        if (response.ok) {
            alert("All messages deleted successfully!");
            document.getElementById("messagesList").innerHTML = ""; // Mesaj listesini temizle
        } else {
            alert("Failed to delete messages.");
        }
    } catch (error) {
        console.error("Error:", error);
        alert("An error occurred while deleting messages.");
    }
});
//#endregion







//#region Contact Form
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
//#endregion