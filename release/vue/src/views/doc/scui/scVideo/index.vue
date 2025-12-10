<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Vedio 视频组件</h1>
                <p>Vedio 组件用于播放视频。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Vedio 组件默认使用示例。">
                    <sc-video ref="video" :src="example1.src" />
                </sc-code>

                <h2>属性</h2>
                <el-table :data="fieldTable" style="width: 100%">
                    <el-table-column prop="name" label="属性名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="type" label="类型" width="80" />
                    <el-table-column prop="val" label="可选值" width="120" />
                    <el-table-column prop="def" label="默认值" width="120" />
                </el-table>
                <h2>事件</h2>
                <el-table :data="eventTable" style="width: 100%">
                    <el-table-column prop="name" label="事件名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="args" label="回调参数" width="120" />
                </el-table>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scVideo from '@/components/scVideo';

export default {
    name: 'scui_video',
    components: {
        scCode,
        scVideo,
    },
    data() {
        return {
            example1: {
                src: '',
                code: `<sc-video ref="video" :src="src" />`,
            },
            fieldTable: [
                { name: 'src', type: 'String', desc: '视频路径', val: '', def: '' },
                { name: 'poster', type: 'String', desc: '视频封面', val: '', def: '' },
                { name: 'volume', type: 'Number', desc: '视频音量', val: '', def: '0.8' },
                { name: 'controls', type: 'Boolean', desc: '是否显示控制条', val: '', def: 'true' },
                { name: 'isLive', type: 'Boolean', desc: '是否直播', val: '', def: 'false' },
                { name: 'autoplay', type: 'Boolean', desc: '是否自动播放', val: '', def: 'false' },
                { name: 'loop', type: 'Boolean', desc: '是否循环播放', val: '', def: 'false' },
                { name: 'videoInit', type: 'Boolean', desc: '是否初始化视频', val: '', def: 'false' },
                { name: 'pip', type: 'Boolean', desc: '是否开启画中画', val: '', def: 'false' },
                { name: 'playbackRate', type: 'Number', desc: '视频播放速率', val: '[0.5, 0.75, 1, 1.5, 2]', def: '1' },
                { name: 'lastPlayTime', type: 'Number', desc: '视频上次播放时间', val: '', def: '0' },
                { name: 'danmu', type: 'Boolean', desc: '是否开启弹幕', val: '', def: 'false' },
                { name: 'resource', type: 'Array', desc: '视频资源列表', val: '', def: '[]' },
                { name: 'progressDot', type: 'Array', desc: '是否显示进度点', val: '', def: '' },
                { name: 'download', type: 'Boolean', desc: '是否开启下载', val: '', def: 'false' },
            ],
            eventTable: [
                { name: 'play', type: 'Function', desc: '视频播放事件', val: '', def: '' },
                { name: 'pause', type: 'Function', desc: '视频暂停事件', val: '', def: '' },
                { name: 'ended', type: 'Function', desc: '视频播放结束事件', val: '', def: '' },
            ]
        };
    },
    methods: {
    }
}
</script>