 // Sidebar navigation
        const sidebarItems = document.querySelectorAll('.sidebar ul li');
        const contentSections = document.querySelectorAll('.content-section');
        const sidebar = document.querySelector('.sidebar');
        const mainContent = document.querySelector('.main-content');

        sidebarItems.forEach(item => {
            item.addEventListener('click', () => {
                if (item.id === 'toggle-sidebar') {
                    sidebar.classList.toggle('collapsed');
                    mainContent.classList.toggle('expanded');
                } else {
                    const section = item.dataset.section;
                    contentSections.forEach(section => section.classList.remove('active'));
                    document.getElementById(section).classList.add('active');
                }
            });
        });

        // Form submission
        const form = document.querySelector('#forms form');
        form.addEventListener('submit', e => {
            e.preventDefault();
            const name = document.getElementById('name').value;
            alert(`Form gönderildi! Gönderen: ${name}`);
        });

        // Charts
        const lineCtx = document.getElementById('lineChart').getContext('2d');
        new Chart(lineCtx, {
            type: 'line',
            data: {
                labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran'],
                datasets: [{
                    label: 'Satışlar',
                    data: [12, 19, 3, 5, 2, 3],
                    backgroundColor: 'rgba(44, 62, 80, 0.2)',
                    borderColor: 'rgba(44, 62, 80, 1)',
                    borderWidth: 1,
                    fill: true
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });

        const barCtx = document.getElementById('barChart').getContext('2d');
        new Chart(barCtx, {
            type: 'bar',
            data: {
                labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran'],
                datasets: [{
                    label: 'Gelir',
                    data: [1200, 1900, 300, 500, 200, 300],
                    backgroundColor: 'rgba(44, 62, 80, 0.8)',
                    borderColor: 'rgba(44, 62, 80, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });