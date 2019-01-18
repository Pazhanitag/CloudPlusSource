import styled from 'vue-styled-components';

const btnProps = { color: String };

// Create a <BrandTab> Vue component that renders a <button> which is
// the color of the current theme
const BrandTabs = styled('div', btnProps)`
.tabs li.is-active {
  border-bottom: 2px solid  ${props => props.color};
  margin-bottom: -1px;
}
.tabs li.is-active a {
  border-bottom-color: ${props => props.color};
  color: ${props => props.color};
  font-weight: bold;
}
`;

export default BrandTabs;
