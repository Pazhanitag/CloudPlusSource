import styled from 'vue-styled-components';

const componentProps = { numOfSteps: Number };

// Create a <BrandStepProgressbar> Vue component that renders a <ul> which is
// used as the cloud plus step progress bar
// the purpose of this component is to adjust the width based on the number of steps
const BrandStepProgressbar = styled('ul', componentProps)`
& li.active~li::after {
  background-color: #dce1ea;
}
`;

export default BrandStepProgressbar;
