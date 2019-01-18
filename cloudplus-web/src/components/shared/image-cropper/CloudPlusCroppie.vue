<template>
  <div :class="{modal: true, 'is-active': showModal}">
    <div class="modal-background"></div>
    <div class="modal-card">
      <section class="modal-card-body">
        <vue-croppie
         ref=croppieRef
        :viewport=" { width: 300, height: 300,type: 'circle'}"
        :enableOrientation="false"
        :enableResize="false"
        @result="result">
        </vue-croppie>
        <div class="button-padding">
          <brand-reverse-primary-btn @click="close" class="is-pulled-left">Cancel</brand-reverse-primary-btn>
          <brand-primary-btn @click="crop" class="is-pulled-right">Crop</brand-primary-btn>
        </div>
      </section>
    </div>
  </div>
</template>
<script>

  export default {
    props: {
      showModal: {
        type: Boolean,
      },
      imageSrc: {
        type: String,
        default: '',
      },
    },
    mounted() {
      this.$refs.croppieRef.bind({
        url: this.imageSrc,
      });
    },
    methods: {
      close() {
        this.showModalLocal = false;
        this.$emit('closeModal');
      },
      crop() {
        const options = {
          format: 'png',
          circle: true,
        };
        this.$refs.croppieRef.result(options);
      },
      result(output) {
        this.showModalLocal = false;
        this.$emit('cropImage', output);
      },
    },
  };
</script>

<style lang="scss" scoped>
  .modal {
    z-index: 9999;
  }
  .modal-card{
    margin: 0;
  }
  .modal-card-body {
    padding-left: 0rem;
    background: transparent;
    width: 400px;
    height: 510px;
    background: white;
    overflow: hidden;
    padding-right: 0px;
    padding-top: 0px;
    border-top-width: 66px;
    margin-top: 0px;
    margin-left: 8rem;
  }

  .button-padding {
    padding-left: 20px;
    padding-bottom: 20px;
    padding-right: 15px;
  }
</style>
