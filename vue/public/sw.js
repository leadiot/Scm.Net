// 缓存名称
const CACHE_NAME = 'scm-cache-v1';
// 需要缓存的资源列表
const urlsToCache = [
    '/',
    '/index.html',
    '/index.css',
    '/index.js',
    '/favicon.ico',
    '/logo/logo-192x192.png',
    '/logo/logo-512x512.png'
];

// 安装阶段：缓存资源
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('缓存已打开');
                return cache.addAll(urlsToCache);
            })
    );
});

// 激活阶段：清理旧缓存
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.filter(cacheName => {
                    return cacheName !== CACHE_NAME;
                }).map(cacheName => {
                    return caches.delete(cacheName);
                })
            );
        })
    );
});

// 拦截请求：从缓存中返回资源
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                // 缓存命中则返回缓存资源，否则请求网络
                return response || fetch(event.request);
            })
    );
});