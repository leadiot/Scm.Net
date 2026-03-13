<template>
    <el-container>
        <el-main class="nopadding">
            <el-empty description="No Favorites!!!" image="/img/nofavorites.svg">
                <h4>您没有收藏的页面!</h4>
                <p>请添加您常用的页面到收藏夹，方便您下次快速访问。</p>
            </el-empty>
        </el-main>
    </el-container>
</template>

<style scoped>
h4 {
    font-size: 20px;
    color: #333;
}

p {
    font-size: 12px;
    color: #999;
    margin: 20px 0;
}
</style>