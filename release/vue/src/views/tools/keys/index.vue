<template>
  <el-container>
    <el-aside>
      列表
    </el-aside>
    <el-main>
      <div>
        <el-form :model="formData" label-width="120px">
          <el-form-item label="字符空间">
            <el-select v-model="formData.set">
              <el-option v-for="item in sets_list" :key="item.id" :label="item.label" :value="item.value" />
            </el-select>
          </el-form-item>
          <el-form-item label="指定字符" v-show="formData.fixs">
            <el-input v-model="formData.chars" />
          </el-form-item>
          <el-form-item label="字符长度">
            <el-input-number v-model="formData.length" :min="4" :max="256" />
          </el-form-item>
          <el-form-item label="可否重复">
            <el-switch v-model="formData.repeat" active-text="是" inactive-text="否" />
          </el-form-item>
          <el-form-item label="生成结果">
            <el-input v-model="formData.result">
              <template #append>
                <el-button :icon="Search" v-copy="formData.result" />
              </template>
            </el-input>
          </el-form-item>
          <el-form-item label="标题">
            <el-input v-model="formData.name" />
          </el-form-item>
          <el-form-item label="网址">
            <el-input v-model="formData.name" />
          </el-form-item>
          <el-form-item label="备注">
            <el-input v-model="formData.remark" type="textarea" :rows="5" />
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="gen">生成</el-button>
            <el-button @click="save">保存</el-button>
          </el-form-item>
        </el-form>
      </div>
    </el-main>
  </el-container>
</template>

<script>
import { JSEncrypt } from 'jsencrypt';

export default {
  name: 'tools_keys',
  data() {
    return {
      formData: {
        name: '',
        url: '',
        set: 1,
        fixs: false,
        chars: '',
        length: 8,
        repeat: true,
        result: '',
        remark: '',
      },
      rules: [

      ],
      sets_list: [{ 'id': 0, 'label': '请选择', 'value': 0 },
      { 'id': 1, 'label': '数字', 'value': 1 },
      { 'id': 2, 'label': '英文字母（大写）', 'value': 2 },
      { 'id': 3, 'label': '英文字母（小写）', 'value': 3 },
      { 'id': 4, 'label': '英文字母（大小写）', 'value': 4 },
      { 'id': 7, 'label': '特殊字符', 'value': 7 },
      { 'id': 5, 'label': '易辨认字符', 'value': 5 },
      { 'id': 6, 'label': '难辨认字符', 'value': 6 },
      { 'id': 8, 'label': '可输入字符', 'value': 8 },
      { 'id': 9, 'label': '指定字符', 'value': 9 },
      ]
    };
  },
  mounted() {
    this.init();
  },
  methods: {
    init() {
      // load key
    },
    gen() {
      if (!this.formData.set) {
        this.$message.warning('请选择字符空间！');
        return;
      }

      var chars = this.getChars();
      var arr = Array.from(chars);
      var tmp = '';
      for (var i = 0; i < this.formData.length; i += 1) {
        var max = arr.length;
        var idx = Math.floor(Math.random() * max);
        tmp += arr[idx];
        if (this.formData.repeat) {
          continue;
        }

        arr.splice(idx, 1);
        if (arr.length < 1) {
          break;
        }
      }

      this.formData.result = tmp;
    },
    getChars() {
      var set = this.formData.set;
      var chars = '';
      // 数字
      if (set == 1) {
        chars += this.addChars(0x30, 0x39);
        return chars;
      }
      // 英文字母（大写）
      if (set == 2) {
        chars += this.addChars(0x41, 0x5A);
        return chars;
      }
      // 英文字母（小写）
      if (set == 3) {
        chars += this.addChars(0x61, 0x7A);
        return chars;
      }
      // 英文字母
      if (set == 4) {
        chars += this.addChars(0x41, 0x5A);
        chars += this.addChars(0x61, 0x7A);
        return chars;
      }
      // 易辨认字符
      if (set == 5) {
        chars += '23456789acdefghijklmnprstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ';
        return chars;
      }
      // 难辨认字符
      if (set == 6) {
        chars += '0O1lI69bqo';
        return chars;
      }
      // 特殊字符
      if (set == 7) {
        chars += this.addChars(0x21, 0x2F);
        chars += this.addChars(0x3A, 0x40);
        chars += this.addChars(0x5B, 0x60);
        chars += this.addChars(0x7B, 0x7E);
        return chars;
      }
      // 可输入字符
      if (set == 8) {
        chars += this.addChars(0x21, 0x7E);
        return chars;
      }
      // 其它字符
      return this.formData.chars;
    },
    addChars(s, t) {
      var c = '';
      while (s <= t) {
        c += String.fromCharCode(s);
        s += 1;
      }
      return c;
    },
    decrypt(msg) {
      const privateKey = '';
      let decrypt = new JSEncrypt();
      decrypt.setPrivateKey(privateKey);
      var decryptMsg = decrypt.decrypt(msg);
      return decryptMsg;
    },
    save() {
      this.gen();
    },
    change(item) {
      // 读取item
      item.result = '';
      // 解密
    }
  },
};
</script>