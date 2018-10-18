// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll('.pagination-link').forEach(l => {
    if (!location.search)
        l.href = '?page=' + l.dataset['page'];
    else
        l.href = location.search.replace(/page=\d+/, 'page=' + l.dataset['page']);
});
