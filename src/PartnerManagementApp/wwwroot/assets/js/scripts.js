

function toggleSideBar() {
    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Remove any existing click event listeners
        sidebarToggle.removeEventListener('click', toggleSidebar);

        // Add a new click event listener
        sidebarToggle.addEventListener('click', toggleSidebar);
    }
}

function toggleSidebar(event) {
    event.preventDefault();
    document.body.classList.toggle('sb-sidenav-toggled');
    localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));

    const sidebarToggleIcon = document.getElementById('sidebarToggleIcon');
    if (sidebarToggleIcon) {
        if (document.body.classList.contains('sb-sidenav-toggled')) {
            sidebarToggleIcon.classList.remove('fa-arrow-left');
            sidebarToggleIcon.classList.add('fa-arrow-right');
        } else {
            sidebarToggleIcon.classList.remove('fa-arrow-right');
            sidebarToggleIcon.classList.add('fa-arrow-left');
        }
    }
}

function showDropdown() {
    const dropdownMenu = document.querySelector('.dropdown-menu');
    if (dropdownMenu) {
        // Remove any existing click event listeners
        dropdownMenu.removeEventListener('click', showDropdown);

        // Add a new click event listener
        dropdownMenu.addEventListener('click', showDropdown);
    }
    dropdownMenu.classList.toggle('show');
    

}

