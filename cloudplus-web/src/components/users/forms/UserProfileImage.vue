<template>
  <div class="content">
    <cloud-plus-browse-image @imageUploaded="avatarUploaded" :croppedImage="croppedImage" :enableCropping="enableCropping" :imgSource="user.profilePicture"></cloud-plus-browse-image>
    <croppie v-if="showModal" :showModal="showModal" @cropImage="cropImage" :imageSrc="imageSrc" @closeModal="closeModal"></croppie>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import Croppie from '@/components/shared/image-cropper/CloudPlusCroppie';
import CloudPlusBrowseImage from '@/components/shared/input-components/CloudPlusBrowseImage';

export default {
  inject: {
    $validator: '$validator',
  },
  components: {
    CloudPlusBrowseImage,
    Croppie,
  },
  props: {
    enableCropping: {
      type: Boolean,
      default: false,
    },
  },
  created() {
    this.$bus.on('openCroppieModal', this.openCroppieModal);
  },
  data() {
    return {
      showModal: false,
      croppedImage: '',
      imageSrc: '',
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/userProfileImage',
    }),
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      unsavedUserChangesPresent: 'user/UNSAVED_USER_CHANGES_PRESENT',
    }),
    avatarUploaded(base64, avatarSrc) {
      this.unsavedUserChangesPresent();
      this.updateUserProperty({
        key: 'avatarBase64',
        value: base64,
      });
      this.updateUserProperty({
        key: 'profilePicture',
        value: avatarSrc,
      });
    },
    closeModal() {
      this.showModal = false;
    },
    cropImage(croppedImage) {
      this.showModal = false;
      this.imageSrc = '';
      this.croppedImage = croppedImage;
    },
    openCroppieModal(image) {
      this.imageSrc = image;
      this.showModal = true;
    },
  },
};
</script>

<style>

</style>
