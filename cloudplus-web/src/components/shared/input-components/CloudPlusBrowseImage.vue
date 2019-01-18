<template>
  <article class="media">
    <div class="media-left">
      <figure class="image" :class="figureSizeClass">
        <img :src="imgSrc" @error="setDefaultPlaceholder" ref="img">
        <p class="help is-danger">{{validationError}}</p>
      </figure>
    </div>
    <div class="media-content">
      <div class="content">
        <p>
          <strong class="title">{{title}}</strong>
          <br>
          <span class="subtitle">{{subtitle}}</span>
        </p>
        <div class="file is-dark">
          <label class="file-label">
            <input class="file-input" type="file" ref="image" v-on:change="upload" accept=".png,.jpg,.jpeg">
            <brand-upload-file-btn>Browse images</brand-upload-file-btn>
          </label>
        </div>
      </div>
      <div @click="removeLogoImage()">
        <brand-color-primary class="remove-logo-link"><a>Remove {{enableCropping ? 'profile' : 'logo'}} image</a></brand-color-primary>
      </div>
    </div>
  </article>
</template>

<script>
import { parseUrlImageDataToBase64String } from '@/helpers/utils';
import { mapGetters } from 'vuex';
import appConfig from 'appConfig';

export default {
  props: {
    figureSizeClass: {
      type: String,
      default: 'is-128x128',
    },
    imgSource: {
      type: String,
      default: appConfig.imagePlaceholder,
    },
    title: {
      type: String,
      default: 'Set profile image',
    },
    subtitle: {
      type: String,
      default: 'JPG, PNG formats allowed with size up to 2MB',
    },
    enableCropping: {
      type: Boolean,
      default: false,
    },
    croppedImage: {
      type: String,
      default: '',
    },
  },
  computed: {
    imgSrc() {
      if (this.img && this.img !== '') {
        return this.img;
      }
      if (this.imgSource === this.branding.logoUrl) {
        this.$emit('imageUploaded', `prepopulatedPicture${this.imgSource.slice(this.imgSource.lastIndexOf('/') + 1)}`, this.imgSource);
      }
      return this.imgSource;
    },
    ...mapGetters({
      branding: 'branding/brand',
    }),
  },
  data() {
    return {
      img: '',
      validationError: '',
    };
  },
  watch: {
    croppedImage() {
      if (this.croppedImage !== '') {
        this.img = this.croppedImage;
        this.$emit('imageUploaded', parseUrlImageDataToBase64String(this.croppedImage), this.croppedImage);
      }
    },
  },
  methods: {
    upload(event) {
      const image = this.$refs.image.files[0];
      const reader = new FileReader();
      if (image.size > 2097152) {
        this.setDefaultPlaceholder();
        if (this.enableCropping) {
          this.$emit('imageUploaded', '', '');
        } else {
          this.$emit('imageUploaded', 'defaultPicture', '');
        }
        this.img = '';
        this.validationError = 'Image size must be less than 2 MB';
      } else {
        this.validationError = '';
        reader.addEventListener('load', () => {
          if (this.enableCropping) {
            this.$bus.emit('openCroppieModal', reader.result);
          } else {
            this.img = reader.result;
            this.$emit('imageUploaded', parseUrlImageDataToBase64String(reader.result), this.img);
          }
        }, false);
        reader.readAsDataURL(image);
        event.target.value = '';
      }
    },
    setDefaultPlaceholder() {
      if (this.enableCropping) {
        this.$refs.img.src = appConfig.imagePlaceholderCircle;
      } else {
        this.$refs.img.src = appConfig.imagePlaceholder;
      }
    },
    removeLogoImage() {
      this.validationError = '';
      this.setDefaultPlaceholder();
      this.$emit('imageUploaded', 'defaultPicture', '');
    },
  },
  mounted() {
    if (this.img && this.img !== '') {
      if (this.enableCropping) {
        this.imgSrc = appConfig.imagePlaceholderCircle;
      } else {
        this.imgSrc = this.img;
      }
    }
  },
};
</script>

<style lang="scss" scoped>
.image {
  display: flex;
  justify-content: center;
  align-items: center;
}
img {
  object-fit: scale-down;
  max-width: 100%;
  max-height: 100%;
}
.title {
  color: $label-color;
  font-size: $secondary-font-size;
}

.subtitle {
  color: $subtitle-color;
  font-size: $secondary-font-size;
}
.remove-logo-link{
  margin-top: 1rem;
  font-size: 0.75rem;
}
</style>
